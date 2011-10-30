using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ManagedDigitalImageProcessing.Filters.EdgeDetectors;

namespace DIPUI.ViewModels
{
    sealed class MainViewModel : ViewModelBase
    {
        public ObservableCollection<EdgeDetector> EdgeDetectors { get; private set; }
        public ObservableCollection<Filter> Filters { get; private set; } 

        public MainViewModel()
        {
            EdgeDetectors = new ObservableCollection<EdgeDetector>
                                {
                                    new CannyEdgeDetector(),
                                    new EdgeDetector("Laplacian Operator", (img =>
                                                                                { 
                                                                                    var filter = new LaplacianOperator();
                                                                                    return filter.Filter(img);
                                                                                })),
                                    new EdgeDetector("Sobel Operator and Non-Maximum Suppression", (img =>
                                                                                {
                                                                                    var sobelOperator = new SobelOperator();
                                                                                    var nonMax = new NonMaximumSuppression();
                                                                                    var output = nonMax.Filter(sobelOperator.FilterSplit(img));
                                                                                    return output.ToPgmImage();
                                                                                })),
                                    new EdgeDetector("Sobel Operator", (img =>
                                                                                {
                                                                                    var sobelOperator = new SobelOperator();
                                                                                    var output = sobelOperator.Filter(img);
                                                                                    return output;
                                                                                })),
                                    new EdgeDetector("None", (img => img))
                                };
            EdgeDetectors.First().Selected = true;

            Filters = new ObservableCollection<Filter>
                          {
                              new MedianFilter(),
                              new AdaptiveMedianFilter(),
                              new GaussianFilter()
                          };
        }

        public EdgeDetector SelectedEdgeDetector
        {
            get { return EdgeDetectors.First(t => t.Selected == true); }
        }

        public IEnumerable<Filter> SelectedFilters
        {
            get { return Filters.Where(f => f.Selected == true); }
        }
    }
}
