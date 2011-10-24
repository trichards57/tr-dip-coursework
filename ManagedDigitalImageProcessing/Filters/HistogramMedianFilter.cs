using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters
{
    /// <summary>
    /// Apply a median filter to the input, using a histogram to optimise the median operation.
    /// </summary>
    /// <remarks>
    /// Operates substantially faster than the MedianFilter, as it doesn't have to sort a list
    /// and only has to recalculate for the part of the window that has changed.
    /// </remarks>
    public class HistogramMedianFilter : FilterBase
    {
        /// <summary>
        /// The size of the filter window.
        /// </summary>
        private int windowSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="HistogramMedianFilter"/> class.
        /// </summary>
        /// <param name="size">The filter window size.</param>
        public HistogramMedianFilter(int size = 3)
        {
            if (size % 2 == 0)
                throw new ArgumentOutOfRangeException("size", windowSize, "The size must be odd.");

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
            var medianPosition = ((windowSize * windowSize) / 2) + 1;

            // Iterate through each column.
            for (var i = 0; i < output.Header.Width; i++)
            {
                // Histogram must be reset at the start of each column.
                // Each element of the array represents a different value in the histogram.
                var histogram = new uint[256];

                // Iterate through each point in the column.
                for (var j = 0; j < output.Header.Height; j++)
                {
                    if (j == 0)
                    {
                        // This is the top of the column, so the entire histogram must be initialised.
                        for (var k = -offset; k <= offset; k++)
                        {
                            for (var l = -offset; l <= offset; l++)
                            {
                                var level = input.Data[calculateIndex(i + k, j + l)];
                                histogram[level]++;
                            }
                        }
                    }
                    else
                    {
                        // Most of the histogram is ready.  Just remove the old top row and add the new bottom row.
                        for (var k = -offset; k <= offset; k++)
                        {
                            // This is the cell in the old top row.
                            var level = input.Data[calculateIndex(i + k, j - offset - 1)];
                            histogram[level]--;
                            // This is the cell in the new bottom row.
                            level = input.Data[calculateIndex(i + k, j + offset)];
                            histogram[level]++;
                        }
                    }

                    uint counter = 0;

                    // Count through the histogram until the median is found or passed.
                    for (int k = 0; k < 256; k++)
                    {
                        counter += histogram[k];
                        if (counter >= medianPosition)
                        {
                            // We've reached the histogram item that contains the median value.
                            // Return it.
                            output.Data[calculateIndex(i, j)] = (byte)k;
                            break;
                        }
                    }
                }
            }

            return output;
        }


        public override string ToString()
        {
            return string.Format("Histogram {0}", windowSize);
        }
    }
}
