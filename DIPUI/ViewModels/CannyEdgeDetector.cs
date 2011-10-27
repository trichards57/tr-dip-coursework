using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIPUI.ViewModels
{
    class CannyEdgeDetector : EdgeDetector
    {
        public CannyEdgeDetector()
            : base("Canny")
        {
        }

        private int _upperThreshold = 100;
        private int _lowerThreshold = 10;

        public int UpperThreshold
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

        public int LowerThreshold
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
