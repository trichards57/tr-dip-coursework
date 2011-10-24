using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    public class EdgeDetector1 : FilterBase
    {
        public override PgmImage Filter(PgmImage input)
        {
            var output = new PgmImage {Header = input.Header, Data = new byte[input.Data.Length]};

            var template = new[] { 2, -1, -1, 0 };

            output.Data = Convolve(template, input.Data, new System.Drawing.Size(2, 2), new System.Drawing.Size(input.Header.Width, input.Header.Height));

            return output;
        }
    }
}
