using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.PGM;
using System.Threading.Tasks;

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    class Dilate : FilterBase
    {
        private int _windowSize;
        private bool _square;

        public Dilate(int functionSize = 3, bool square = false)
        {
            _windowSize = functionSize;
            _square = square;
        }

        public PgmImage Filter(PgmImage input)
        {
            // Partial function application to simplify index calculation.
            Func<int, int, int> calculateIndex = ((x, y) => CalculateIndex(x, y, input.Header.Width, input.Header.Height));

            var output = new PgmImage { Header = input.Header, Data = new byte[input.Data.Length] };

            var offset = _windowSize / 2;

            // Iterate through each column.
            Parallel.For(0, output.Header.Width, i =>
            {
                // Iterate through each point in the column.
                for (var j = 0; j < output.Header.Height; j++)
                {
                    byte max = 0;
                    // Iterate through each of the items in the window, adding the value to a list.
                    for (var k = -offset; k <= offset; k++)
                    {
                        for (var l = -offset; l <= offset; l++)
                        {
                            if (_square || Math.Sqrt(k * k + l * l) < offset)
                            {
                                var value = input.Data[calculateIndex(i + k, j + l)];
                                if (value > max)
                                    max = value;
                            }
                        }
                    }
                    
                    // Take the middle value (the median) and insert it in to the new image.
                    output.Data[calculateIndex(i, j)] = max;
                }
            });

            return output;
        }
    }
}
