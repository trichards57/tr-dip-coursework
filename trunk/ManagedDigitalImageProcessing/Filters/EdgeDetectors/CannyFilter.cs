namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    public sealed class CannyFilter : FilterBase
    {
        private readonly HysteresisThresholding _hysteresis;
        private readonly NonMaximumSuppression _nonMaximal;
        private readonly SobelOperator _edgeFilter;
        private readonly byte _highT;
        private readonly byte _lowT;

        public CannyFilter(byte highThreshold, byte lowThreshold)
        {
            _hysteresis = new HysteresisThresholding(highThreshold, lowThreshold);
            _nonMaximal = new NonMaximumSuppression();
            _edgeFilter = new SobelOperator();
            _highT = highThreshold;
            _lowT = lowThreshold;
        }

        public override PGM.PgmImage Filter(PGM.PgmImage input)
        {
            return _hysteresis.Filter(_nonMaximal.Filter(_edgeFilter.FilterSplit(input)).ToPgmImage());
        }

        public override string ToString()
        {
            return string.Format("Canny {0} {1}", _highT, _lowT);
        }
    }
}
