using ManagedDigitalImageProcessing.Filters.NoiseReduction;
using ManagedDigitalImageProcessing.PGM;

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
                                      var output = new PgmImage {Header = img.Header, Data = new byte[img.Data.Length]};
                                      NativeNoiseFilters.AdaptiveHistogramMedianFilter(img.Data.Length, img.Data, output.Data,
                                                                                  img.Header.Width, img.Header.Height,
                                                                                  Parameter, CenterValue, ScalingValue);
                                      return output;
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
