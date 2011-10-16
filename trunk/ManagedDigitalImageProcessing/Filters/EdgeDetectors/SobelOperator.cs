using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.PGM;
using System.Threading.Tasks;
using System.Threading;

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    class SobelOperator : FilterBase
    {
        public override PGM.PgmImage Filter(PGM.PgmImage input)
        {
            var output = new PgmImage();
            output.Header = input.Header;
            output.Data = new byte[input.Header.Width * input.Header.Height];

            var outputTemp = FilterSplit(input);

            for (var i = 0; i < input.Data.Length; i++)
                output.Data[i] = (byte)Math.Sqrt(outputTemp.XData[i] * outputTemp.XData[i] + outputTemp.XData[i] * outputTemp.XData[i]);

            return output;
        }

        public FilterResult FilterSplit(PgmImage input)
        {
            var output = new FilterResult();
            output.Header = input.Header;

            var template1 = new int[] { 1, 0, -1, 2, 0, -2, 1, 0, -1 };
            var template2 = new int[] { 1, 2, 1, 0, 0, 0, -1, -2, -1 };

            output.XData = Convolve(template1, input.Data, new System.Drawing.Size(3, 3), new System.Drawing.Size(input.Header.Width, input.Header.Height));
            output.YData = Convolve(template2, input.Data, new System.Drawing.Size(3, 3), new System.Drawing.Size(input.Header.Width, input.Header.Height));

            return output;
        }
    }
}
