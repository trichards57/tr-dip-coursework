﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    public class LaplacianOperator : FilterBase
    {
        public override PgmImage Filter(PgmImage input)
        {
            var output = new PgmImage();
            output.Header = input.Header;
            output.Data = new byte[input.Data.Length];

            Func<int, int, int> calculateIndex = ((x, y) => CalculateIndex(x, y, input.Header.Width, input.Header.Height));

            var template = new int[] { 0, -1, 0, -1, 4, -1, 0, -1, 0 };

            output.Data = Convolve(template, input.Data, new System.Drawing.Size(3, 3), new System.Drawing.Size(input.Header.Width, input.Header.Height));

            return output;
        }

        public override string ToString()
        {
            return "Laplace";
        }
    }
}
