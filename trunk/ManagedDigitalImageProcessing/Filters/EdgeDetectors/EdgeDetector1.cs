using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    class EdgeDetector1 : FilterBase
    {
        public override PgmImage Filter(PgmImage input)
        {
            var output = new PgmImage();
            output.Header = input.Header;
            output.Data = new byte[input.Data.Length];

            Func<int, int, int> calculateIndex = ((x, y) => CalculateIndex(x, y, input.Header.Width, input.Header.Height));

            var template = new int[] { 2, -1, -1, 0 };

            output.Data = Convolve(template, input.Data, new System.Drawing.Size(2, 2), new System.Drawing.Size(input.Header.Width, input.Header.Height));

            return output;
        }
    }
}
