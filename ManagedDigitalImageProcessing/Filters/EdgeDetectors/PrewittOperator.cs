using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    class PrewittOperator : FilterBase
    {
        public override PGM.PgmImage Filter(PGM.PgmImage input)
        {
            var output = new PgmImage();
            output.Header = input.Header;
            output.Data = new byte[input.Data.Length];

            var template1 = new int[] { 1, 0, -1, 1, 0, -1, 1, 0, -1 };
            var template2 = new int[] { 1, 1, 1, 0, 0, 0, -1, -1, -1 };

            var val1 = Convolve(template1, input.Data, new System.Drawing.Size(3, 3), new System.Drawing.Size(input.Header.Width, input.Header.Height));
            var val2 = Convolve(template2, input.Data, new System.Drawing.Size(3, 3), new System.Drawing.Size(input.Header.Width, input.Header.Height));

            for (var i = 0; i < val1.Length; i++)
                output.Data[i] = Math.Max(val1[i], val2[i]);

            return output;
        }
    }
}
