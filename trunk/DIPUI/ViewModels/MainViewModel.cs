using System.Collections.ObjectModel;

namespace DIPUI.ViewModels
{
    class MainViewModel : ViewModelBase 
    {
        public ObservableCollection<EdgeDetector> EdgeDetectors { get; set; }

        public MainViewModel()
        {
            EdgeDetectors = new ObservableCollection<EdgeDetector>
                                {
                                    new CannyEdgeDetector(),
                                    new EdgeDetector("Laplacian Operator"),
                                    new EdgeDetector("Sobel Operator and Non-Maximum Suppression"),
                                    new EdgeDetector("Sobel Operator"),
                                    new EdgeDetector("None")
                                };
        }
    }
}
