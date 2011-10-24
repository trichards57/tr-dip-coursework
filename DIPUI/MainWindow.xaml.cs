using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading.Tasks;
using ManagedDigitalImageProcessing.PGM;
using System.IO;
using System.Windows.Interop;
using ManagedDigitalImageProcessing.Filters;
using ManagedDigitalImageProcessing.Filters.EdgeDetectors;

namespace DIPUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PgmImage input;
        PgmImage output;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void StartBusy()
        {
            var action = new Action(() => UpdateButton.IsEnabled = false);
            Dispatcher.Invoke(action);
        }

        public void StopBusy()
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
                    input = PgmLoader.LoadImage(inFile);
                }

                var action = new Action(() =>
                {
                    var bitmap = input.ToBitmap();
                    var handle = bitmap.GetHbitmap();
                    var source = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));
                    InputImage.Source = source;
                });
                Dispatcher.Invoke(action);
                StopBusy();
            });

            task.Start();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var filterTasks = new List<FilterTask>();

            Func<object, PgmImage> getImage = (img =>
            {
                if (img is PgmImage)
                    return (PgmImage)img;
                else if (img is Task<PgmImage>)
                    return ((Task<PgmImage>)img).Result;
                else
                    return null;
            });
                        
            
            if (EnableMedianCheckBox.IsChecked == true)
            {
                var windowSize = int.Parse(MedianSizeTextBox.Text);
                Func<object, PgmImage> f = (img =>
                {
                    var data = getImage(img);
                    var filter = new HistogramMedianFilter(windowSize);
                    return filter.Filter(data);
                }
                        );
                var priority = int.Parse(MedianPriorityTextBox.Text);
                filterTasks.Add(new FilterTask { Task = f, Priority = priority });
            }
            if (EnableGaussianCheckBox.IsChecked == true)
            {
                var windowSize = double.Parse(GaussianSizeTextBox.Text);
                Func<object, PgmImage> f = (img =>
                {
                    var data = getImage(img);
                    var filter = new SeperatedGaussianFilter(windowSize);
                    return filter.Filter(data);
                });
                var priority = int.Parse(GaussianPriorityTextBox.Text);
                filterTasks.Add(new FilterTask { Task = f, Priority = priority });
            }

            int currentPriority = 0;
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

            if (EnableCannyRadioButton.IsChecked == true)
            {
                var highT = byte.Parse(CannyUpperThresholdComboBox.Text);
                var lowT = byte.Parse(CannyLowerThresholdComboBox.Text);

                Func<object, PgmImage> f = (img =>
                    {
                        var data = getImage(img); 
                        var filter = new CannyFilter(highT, lowT);
                        return filter.Filter(data);
                    });
                filterTasks.Add(new FilterTask { Task = f, Priority = currentPriority++ });
            }
            else if (EnableLaplacianRadioButton.IsChecked == true)
            {
                Func<object, PgmImage> f = (img =>
                {
                    var data = getImage(img);
                    var filter = new LaplacianOperator();
                    return filter.Filter(data);
                });
                filterTasks.Add(new FilterTask { Task = f, Priority = currentPriority++ });
            }
            else if (NonMaximalSuppressionOnlyRadioButton.IsChecked == true)
            {
                Func<object, PgmImage> f = (img =>
                    {
                        var data = getImage(img);
                        var sobelOperator = new SobelOperator();
                        var nonMax = new NonMaximumSuppression();
                        var output = nonMax.Filter(sobelOperator.FilterSplit(data));
                        return output.ToPgmImage();
                    });
                filterTasks.Add(new FilterTask { Task = f, Priority = currentPriority++ });
            }
            else if (SobelOnlyRadioButton.IsChecked == true)
            {
                Func<object, PgmImage> f = (img =>
                    {
                        var data = getImage(img);
                        var sobelOperator = new SobelOperator();
                        var output = sobelOperator.Filter(data);
                        return output;
                    });
                filterTasks.Add(new FilterTask { Task = f, Priority = currentPriority++ });
            }

            Action<object> finalF = (img =>
            {
                output = ((Task<PgmImage>)img).Result;
                var action = new Action(() =>
                {
                    var bitmap = output.ToBitmap();
                    var handle = bitmap.GetHbitmap();
                    var source = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));
                    OutputImage.Source = source;
                });
                Dispatcher.Invoke(action);
                StopBusy();
            });

            var orderedTasks = filterTasks.OrderBy(t => t.Priority);
            if (filterTasks.Count() > 0)
            {
                var startTask = new Task<PgmImage>(orderedTasks.First().Task, input);
                var currentTask = startTask;

                foreach (var t in orderedTasks.Skip(1))
                {
                    currentTask = currentTask.ContinueWith(t.Task);
                }


                currentTask.ContinueWith(finalF);

                StartBusy();
                startTask.Start();
            }
        }

        struct FilterTask
        {
            public Func<object, PgmImage> Task;
            public int Priority;
        }

    }
}
