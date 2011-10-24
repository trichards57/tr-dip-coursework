using System;
using System.Linq;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    public class SobelOperator : FilterBase
    {
        public override PgmImage Filter(PgmImage input)
        {
            var output = new PgmImage {Header = input.Header, Data = new byte[input.Header.Width*input.Header.Height]};

            var outputTemp = FilterSplit(input);

            for (var i = 0; i < input.Data.Length; i++)
                output.Data[i] = (byte)Math.Sqrt(outputTemp.XData[i] * outputTemp.XData[i] + outputTemp.XData[i] * outputTemp.XData[i]);

            return output;
        }

        public FilterResult FilterSplit(PgmImage input)
        {
            var output = new FilterResult {Header = input.Header};

            var template1 = new double[] { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
            var template2 = new double[] { 1, 2, 1, 0, 0, 0, -1, -2, -1 };

            var data = Convolve(template1, input.Data.Select(t => (int)t).ToArray(), new System.Drawing.Size(3, 3), new System.Drawing.Size(input.Header.Width, input.Header.Height));
            checked
            {
                var max = data.Max(i => Math.Abs(i));
                var data1 = data.AsParallel().AsOrdered().Select(i => (byte)(Math.Abs(i * 255 / max))).ToArray();
                output.XData = data1;
            }

            data = Convolve(template2, input.Data.Select(t => (int)t).ToArray(), new System.Drawing.Size(3, 3), new System.Drawing.Size(input.Header.Width, input.Header.Height));
            checked
            {
                var max = data.Max(i => Math.Abs(i));
                var data1 = data.AsParallel().AsOrdered().Select(i => (byte)(Math.Abs(i * 255 / max))).ToArray();
                output.YData = data1;
            }

            return output;
        }
    }
}
