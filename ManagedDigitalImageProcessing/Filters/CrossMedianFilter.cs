using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters
{
    /// <summary>
    /// Apply a median filter to the input, using only squares immediately above and immediately below the input.
    /// </summary>
    class CrossMedianFilter : FilterBase
    {
        /// <summary>
        /// The size of the filter window.
        /// </summary>
        private int windowSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossMedianFilter"/> class.
        /// </summary>
        /// <param name="size">The filter window size.</param>
        public CrossMedianFilter(int size = 3)
        {
            if (size % 2 == 0)
                throw new ArgumentOutOfRangeException("size", size, "The size must be odd.");

            windowSize = size;
        }

        /// <summary>
        /// Filters the specified input.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <returns>The filtered image.</returns>
        public override PgmImage Filter(PgmImage input)
        {
            var output = new PgmImage();
            output.Header = input.Header;
            output.Data = new byte[input.Data.Length];

            var offset = windowSize / 2;

            // Partial function application to simplify index calculation.
            Func<int, int, int> calculateIndex = ((x, y) => CalculateIndex(x, y, input.Header.Width, input.Header.Height));

            // Iterate through each column.
            for (var i = 0; i < output.Header.Width; i++)
            {
                // Iterate through each point in the column.
                for (var j = 0; j < output.Header.Height; j++)
                {
                    var list = new List<byte>();
                    // Iterate through the points along the horizontal.
                    for (var k = -offset; k <= offset; k++)
                    {
                        // Add the value of the point to the list.
                        list.Add(input.Data[calculateIndex(i + k, j)]);
                    }
                    // Do the same for the points along the vertical.
                    for (var l = -offset; l <= offset; l++)
                    {
                        if (l != 0) // Added to avoid duplicating the center point.
                            list.Add(input.Data[calculateIndex(i, j + l)]);
                    }
                    // Sort the list.
                    list.Sort();
                    // Take the middle value (the median) and insert it in to the new image.
                    output.Data[calculateIndex(i, j)] = list[list.Count / 2];
                }
            }
            return output;
        }

    }
}
