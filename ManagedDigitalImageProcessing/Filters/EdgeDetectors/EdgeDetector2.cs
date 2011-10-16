using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    class EdgeDetector2 : FilterBase
    {

        public override PGM.PgmImage Filter(PGM.PgmImage input)
        {
            var output = new PgmImage();
            output.Header = input.Header;
            output.Data = new byte[input.Data.Length];

            var template = new int[] { 0, 1, 0, 1, 0, -1, 0, -1, 0 };

            output.Data = Convolve(template, input.Data, new System.Drawing.Size(3, 3), new System.Drawing.Size(input.Header.Width, input.Header.Height));

            return output;
        }
    }
}
