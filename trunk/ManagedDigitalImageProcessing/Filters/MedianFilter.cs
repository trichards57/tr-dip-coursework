using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters
{
    class CrossMedianFilter : FilterBase
    {
        private int windowSize;

        public CrossMedianFilter(int size = 3)
        {
            if (size % 2 == 0)
                throw new ArgumentOutOfRangeException("size", windowSize, "The size must be odd.");

            windowSize = size;
        }

        public override PgmImage Filter(PgmImage input)
        {
            var output = new PgmImage();
            output.Header = input.Header;
            output.Data = new byte[input.Data.Length];

            var offset = windowSize / 2;

            for (var i = offset; i < output.Header.Width - offset; i++)
            {
                for (var j = offset; j < output.Header.Height - offset; j++)
                {
                    var list = new List<byte>();
                    for (var k = -offset; k <= offset; k++)
                    {
                        list.Add(input.Data[CalculateIndex(i + k, j, output.Header.Width)]);
                    }
                    for (var l = -offset; l <= offset; l++)
                    {
                        if (l != 0) // Added to avoid duplicating the center point.
                            list.Add(input.Data[CalculateIndex(i, j + l, output.Header.Width)]);
                    }


                    list.Sort();
                    output.Data[CalculateIndex(i, j, output.Header.Width)] = list[4];
                }
            }

            return output;
        }

    }
}
