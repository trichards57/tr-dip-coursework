using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    class RobertsCrossOperator : FilterBase
    {
        public override PgmImage Filter(PgmImage input)
        {
            var output = new PgmImage();
            output.Header = input.Header;
            output.Data = new byte[input.Data.Length];

            Func<int, int, int> calculateIndex = ((x, y) => CalculateIndex(x, y, input.Header.Width, input.Header.Height));

            var template1 = new int[] { 1, 0, 0, -1 };
            var template2 = new int[] { 0, 1, -1, 0 };

            var val1 = Convolve(template1, input.Data, new System.Drawing.Size(2, 2), new System.Drawing.Size(input.Header.Width, input.Header.Height));
            var val2 = Convolve(template2, input.Data, new System.Drawing.Size(2, 2), new System.Drawing.Size(input.Header.Width, input.Header.Height));

            for (var i = 0; i < val1.Length; i++)
                output.Data[i] = Math.Max(val1[i], val2[i]);

            return output;
        }
    }
}
