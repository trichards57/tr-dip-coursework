using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    public sealed class EdgeDetector2 : FilterBase
    {
        public override PgmImage Filter(PgmImage input)
        {
            var output = new PgmImage {Header = input.Header, Data = new byte[input.Data.Length]};

            var template = new[] { 0, 1, 0, 1, 0, -1, 0, -1, 0 };

            output.Data = Convolve(template, input.Data, new System.Drawing.Size(3, 3), new System.Drawing.Size(input.Header.Width, input.Header.Height));

            return output;
        }
    }
}
