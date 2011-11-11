using ManagedDigitalImageProcessing.Filters.NoiseReduction;

namespace DIPUI.ViewModels
{
    sealed class AdaptiveMedianFilter : Filter
    {
        private double _centerValue;
        private double _scalingValue;

        public AdaptiveMedianFilter()
            : base("Median Filter", "Window Size")
        {
            FilterFunction = (img =>
                                  {
                                      var filter = new AdaptiveHistogramMedianFilter(CenterValue, ScalingValue,
                                                                                     Parameter);
                                      return filter.Filter(img);
                                  });
        }

        public double CenterValue
        {
            get { return _centerValue; }
            set
            {
                _centerValue = value;
                OnPropertyChanged("CenterValue");
            }
        }

        public double ScalingValue
        {
            get { return _scalingValue; }
            set
            {
                _scalingValue = value;
                OnPropertyChanged("ScalingValue");
            }
        }
        
    }
}
