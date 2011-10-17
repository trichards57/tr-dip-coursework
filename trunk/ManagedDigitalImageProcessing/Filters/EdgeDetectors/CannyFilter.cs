using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    class CannyFilter : FilterBase
    {
        private HysteresisThresholding hysteresis;
        private NonMaximumSuppression nonMaximal;
        private SobelOperator edgeFilter;
        private byte highT;
        private byte lowT;

        public CannyFilter(byte highThreshold, byte lowThreshold)
        {
            hysteresis = new HysteresisThresholding(highThreshold, lowThreshold);
            nonMaximal = new NonMaximumSuppression();
            edgeFilter = new SobelOperator();
            highT = highThreshold;
            lowT = lowThreshold;
        }

        public override PGM.PgmImage Filter(PGM.PgmImage input)
        {
            return hysteresis.Filter(nonMaximal.Filter(edgeFilter.FilterSplit(input)).ToPgmImage());
        }

        public override string ToString()
        {
            return string.Format("Canny {0} {1}", highT, lowT);
        }
    }
}
