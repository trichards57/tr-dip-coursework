using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.PGM;
using System.Threading.Tasks;

namespace ManagedDigitalImageProcessing.Filters
{
    /// <summary>
    /// Apply a median filter to the input.
    /// </summary>
    class MedianFilter : FilterBase
    {
        /// <summary>
        /// The size of the filter window.
        /// </summary>
        private int windowSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="MedianFilter"/> class.
        /// </summary>
        /// <param name="size">The filter window size.</param>
        public MedianFilter(int size = 3)
        {
            if (size % 2 == 0)
                throw new ArgumentOutOfRangeException("size", size, "The size must be odd.");

            windowSize = size;
        }

        /// <summary>
        /// Filters the specified input.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <returns>
        /// The filtered image.
        /// </returns>
        public override PgmImage Filter(PgmImage input)
        {
            var output = new PgmImage();
            output.Header = input.Header;
            output.Data = new byte[input.Data.Length];

            // Partial function application to simplify index calculation.
            Func<int, int, int> calculateIndex = ((x, y) => CalculateIndex(x, y, input.Header.Width, input.Header.Height));

            var offset = windowSize / 2;

            // Iterate through each column.
            Parallel.For(0, output.Header.Width, i =>
            {
                // Iterate through each point in the column.
                for (var j = 0; j < output.Header.Height; j++)
                {
                    var list = new List<byte>();
                    // Iterate through each of the items in the window, adding the value to a list.
                    for (var k = -offset; k <= offset; k++)
                    {
                        for (var l = -offset; l <= offset; l++)
                        {
                            list.Add(input.Data[calculateIndex(i + k, j + l)]);
                        }
                    }
                    // Sort the list.
                    list.Sort();
                    // Take the middle value (the median) and insert it in to the new image.
                    output.Data[calculateIndex(i, j)] = list[list.Count / 2];
                }
            });

            return output;
        }

    }
}
