using System;
using ManagedDigitalImageProcessing.Filters.EdgeDetectors;

namespace DIPUI.ViewModels
{
    sealed class CannyEdgeDetector : EdgeDetector
    {
        public CannyEdgeDetector()
            : base("Canny")
        {
        }

        public override Func<ManagedDigitalImageProcessing.PGM.PgmImage, ManagedDigitalImageProcessing.PGM.PgmImage> FilterFunction
        {
            get
            {
                return (img =>
                            {
                                var filter = new CannyFilter(UpperThreshold, LowerThreshold);
                                return filter.Filter(img);
                            });
            }
            protected set
            {

            }
        }

        private byte _upperThreshold = 100;
        private byte _lowerThreshold = 10;

        public byte UpperThreshold
        {
            get { return _upperThreshold; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", value, "Value must be greater than 0.");
                _upperThreshold = value;
                OnPropertyChanged("UpperThreshold");
                if (UpperThreshold < LowerThreshold)
                    LowerThreshold = UpperThreshold;
            }
        }

        public byte LowerThreshold
        {
            get { return _lowerThreshold; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", value, "Value must be greater than 0.");
                _lowerThreshold = value;
                OnPropertyChanged("LowerThreshold");
                if (UpperThreshold < LowerThreshold)
                    UpperThreshold = LowerThreshold;
            }
        }
    }
}
