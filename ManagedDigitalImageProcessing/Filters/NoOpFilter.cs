namespace ManagedDigitalImageProcessing.Filters
{
    public sealed class NoOpFilter : FilterBase
    {
        public override PGM.PgmImage Filter(PGM.PgmImage input)
        {
            return input;
        }

        public override string ToString()
        {
            return "NoOp";
        }
    }
}
