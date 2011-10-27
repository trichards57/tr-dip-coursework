using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ManagedDigitalImageProcessing.PGM;

namespace DIPUI.ViewModels
{
    class MainViewModel : ViewModelBase 
    {
        public ObservableCollection<EdgeDetector> EdgeDetectors { get; set; }

        public MainViewModel()
        {
            Func<object, PgmImage> getImage = (img =>
            {
                if (img is PgmImage)
                    return (PgmImage)img;
                if (img is Task<PgmImage>)
                    return ((Task<PgmImage>)img).Result;
                return null;
            });

            EdgeDetectors = new ObservableCollection<EdgeDetector>
                                {
                                    new CannyEdgeDetector(),
                                    new EdgeDetector("Laplacian Operator"),
                                    new EdgeDetector("Sobel Operator and Non-Maximum Suppression"),
                                    new EdgeDetector("Sobel Operator"),
                                    new EdgeDetector("None")
                                };
            EdgeDetectors.First().Selected = true;
        }


    }
}
