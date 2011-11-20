// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistogramMedianFilter.cs" company="Tony Richards">
//   Copyright (c) 2011, Tony Richards
//   All rights reserved.
//
//   Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
//   following conditions are met:
//
//   Redistributions of source code must retain the above copyright notice, this list of conditions and the following 
//   disclaimer.
//   
//   Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the 
//   following disclaimer in the documentation and/or other materials provided with the distribution.
//   
//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
//   INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
//   DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
//   SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
//   SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//   WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE 
//   USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// <summary>
//   Apply a median filter to the input, using a histogram to optimise the median operation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    using System;
    using System.Threading.Tasks;

    using ManagedDigitalImageProcessing.Images;

    /// <summary>
    /// Apply a median filter to the input, using a histogram to optimise the median operation.
    /// </summary>
    /// <remarks>
    /// Operates substantially faster than the MedianFilter, as it doesn't have to sort a list
    /// and only has to recalculate for the part of the window that has changed.
    /// </remarks>
    public static class HistogramMedianFilter
    {
        /// <summary>
        /// Filters the specified input.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <param name="windowSize">Size of the filter window.</param>
        /// <returns>
        /// The filtered image.
        /// </returns>
        public static ImageData Filter(ImageData input, int windowSize)
        {
            if (windowSize % 2 == 0)
            {
                throw new ArgumentOutOfRangeException("windowSize", windowSize, "The windowSize must be odd.");
            }

            var output = new ImageData(input.Width, input.Height);

            // Partial function application to simplify index calculation.
            Func<int, int, int> calculateIndex = (x, y) => ImageUtilities.CalculateIndex(x, y, input.Width, input.Height);
            Func<int, int, int> calculateOutputIndex = (x, y) => ImageUtilities.CalculateIndex(x, y, output.Width);

            var offset = windowSize / 2;
            var medianPosition = ((windowSize * windowSize) / 2) + 1;

            // Iterate through each column.
            Parallel.For(
                0,
                output.Width,
                i =>
                {
                    // Histogram must be reset at the start of each column.
                    // Each element of the array represents a different value in the histogram.
                    var histogram = new uint[256];

                    // Iterate through each point in the column.
                    for (var j = 0; j < output.Height; j++)
                    {
                        if (j == 0)
                        {
                            // This is the top of the column, so the entire histogram must be initialised.
                            for (var k = -offset; k <= offset; k++)
                            {
                                for (var l = -offset; l <= offset; l++)
                                {
                                    var level =
                                        input.Data[calculateIndex(i + k, j + l)];
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
                        for (var k = 0; k < 256; k++)
                        {
                            counter += histogram[k];
                            if (counter < medianPosition)
                            {
                                continue;
                            }

                            // We've reached the histogram item that contains the median value.
                            // Return it.
                            output.Data[calculateOutputIndex(i, j)] = k;
                            break;
                        }
                    }
                });

            return output;
        }
    }
}
