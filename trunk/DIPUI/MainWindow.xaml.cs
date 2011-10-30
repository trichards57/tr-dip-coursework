using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using DIPUI.ViewModels;
using ManagedDigitalImageProcessing.Filters;
using ManagedDigitalImageProcessing.Filters.NoiseReduction;
using ManagedDigitalImageProcessing.PGM;

namespace DIPUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    internal sealed partial class MainWindow
    {
        PgmImage _input;
        PgmImage _output;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartBusy()
        {
            var action = new Action(() => UpdateButton.IsEnabled = false);
            Dispatcher.Invoke(action);
        }

        private void StopBusy()
        {
            var action = new Action(() => UpdateButton.IsEnabled = true);
            Dispatcher.Invoke(action);
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var task = new Task(() =>
            {
                StartBusy();
                using (var inFile = File.Open(@"..\..\..\Base Images\foetus.pgm", FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    _input = PgmLoader.LoadImage(inFile);
                }

                var action = new Action(() =>
                {
                    var bitmap = _input.ToBitmap();
                    var handle = bitmap.GetHbitmap();
                    var source = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));
                    InputImage.Source = source;
                });
                Dispatcher.Invoke(action);
                StopBusy();
            });

            task.Start();
        }

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            Func<object, PgmImage> getImage = (img =>
                                                   {
                                                       if (img is PgmImage)
                                                           return (PgmImage)img;
                                                       if (img is Task<PgmImage>)
                                                           return ((Task<PgmImage>)img).Result;
                                                       return null;
                                                   });

            var datacontext = (MainViewModel)FindResource("dataContext");

            var filterTasks = (from f in datacontext.SelectedFilters
                               let action = (Func<object, PgmImage>) (img =>
                                                                          {
                                                                              var data = getImage(img);
                                                                              return f.FilterFunction(data);
                                                                          })
                               select new FilterTask {Task = action, Priority = f.Priority}).ToList();

            var currentPriority = 0;
            if (filterTasks.Count > 0)
                currentPriority = filterTasks.Max(t => t.Priority) + 1;

            if (EnableBitSliceCheckBox.IsChecked == true)
            {
                var level = byte.Parse(BitSliceLevelTextBox.Text, System.Globalization.NumberStyles.HexNumber | System.Globalization.NumberStyles.AllowHexSpecifier);
                Func<object, PgmImage> f = (img =>
                {
                    var data = getImage(img);
                    var filter = new BitwiseAndFilter(level);
                    return filter.Filter(data);
                });
                filterTasks.Add(new FilterTask { Task = f, Priority = currentPriority++ });
            }



            Func<object, PgmImage> func = (img =>
                                            {
                                                var data = getImage(img);
                                                var edgeDetector = datacontext.SelectedEdgeDetector;
                                                return edgeDetector.FilterFunction(data);
                                            });
            filterTasks.Add(new FilterTask { Task = func, Priority = currentPriority });

            Action<object> finalF = (img =>
            {
                _output = ((Task<PgmImage>)img).Result;
                var action = new Action(() =>
                {
                    var bitmap = _output.ToBitmap();
                    var handle = bitmap.GetHbitmap();
                    var source = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));
                    OutputImage.Source = source;
                });
                Dispatcher.Invoke(action);
                StopBusy();
            });

            var orderedTasks = filterTasks.OrderBy(t => t.Priority);
            if (filterTasks.Count() <= 0) return;
            var startTask = new Task<PgmImage>(orderedTasks.First().Task, _input);

            var currentTask = orderedTasks.Skip(1).Aggregate(startTask, (current, t) => current.ContinueWith(t.Task));


            currentTask.ContinueWith(finalF);

            StartBusy();
            startTask.Start();
        }

        struct FilterTask
        {
            public Func<object, PgmImage> Task;
            public int Priority;
        }

    }
}
