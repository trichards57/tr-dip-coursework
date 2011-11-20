// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdaptiveHistogramMedianFilter.cs" company="Tony Richards">
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
//   Apply an adaptive median filter to the input, using a histogram to optimise the median operation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    using System;
    using System.Threading.Tasks;

    using Images;

    /// <summary>
    /// Apply an adaptive median filter to the input, using a histogram to optimise the median operation.
    /// </summary>
    /// Algorithm taken from @cite adaptiveMedianPaper
    public static class AdaptiveHistogramMedianFilter
    {
        /// <summary>
        /// Filters the specified input.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <param name="centreWeight">The centre weight of the template.</param>
        /// <param name="scalingConstant">The scaling constant used to create the template.</param>
        /// <param name="templateSize">The size of the template.</param>
        /// <returns>
        /// The filtered image.
        /// </returns>
        public static ImageData Filter(ImageData input, double centreWeight, double scalingConstant, int templateSize = 3)
        {
            if (templateSize % 2 == 0)
            {
                throw new ArgumentOutOfRangeException("templateSize", templateSize, "The size must be odd.");
            }

            var output = new ImageData(input.Width, input.Height);

            // Convenience function to simplify index calculation.
            Func<int, int, int> calculateIndex = (x, y) => ImageUtilities.CalculateIndex(x, y, input.Width, input.Height);
            Func<int, int, int> calculateOutputIndex = (x, y) => ImageUtilities.CalculateIndex(x, y, output.Width);

            var offset = templateSize / 2;
            var itemCount = templateSize * templateSize;

            // Iterate through each column.
            Parallel.For(
                0,
                output.Width,
                i =>
                {
                    // Iterate through each point in the column.
                    for (var j = 0; j < output.Height; j++)
                    {
                        var sum = 0.0;

                        // First take the sum of all the pixels
                        for (var k = -offset; k <= offset; k++)
                        {
                            for (var l = -offset; l <= offset; l++)
                            {
                                var level = input.Data[calculateIndex(i + k, j + l)];
                                sum += level;
                            }
                        }

                        // Calculate the mean
                        var mean = sum / itemCount;
                        var squareDiffTotal = 0.0;

                        // Calculate the variance of the window
                        for (var k = -offset; k <= offset; k++)
                        {
                            for (var l = -offset; l <= offset; l++)
                            {
                                var level = input.Data[calculateIndex(i + k, j + l)];
                                var diff = level - mean;
                                squareDiffTotal += diff * diff;
                            }
                        }

                        var variance = squareDiffTotal / itemCount;
                        
                        uint weightSum = 0;

                        // A new histogram is required for each window position.
                        var histogram = new uint[256];
                        // Populate the histogram, weighting the values based on the variance
                        for (var k = -offset; k <= offset; k++)
                        {
                            for (var l = -offset; l <= offset; l++)
                            {
                                var level = input.Data[calculateIndex(i + k, j + l)];
                                var distance = Math.Sqrt((k * k) + (l * l));
                                var weight = centreWeight - (scalingConstant * distance * variance / mean);
                                // Ensure that the weight is not negative
                                weight = weight < 0 ? 0 : Math.Round(weight);
                                histogram[level] += (uint)weight;
                                // Calculate the total number of entries added to the histogram 
                                weightSum += (uint)weight;
                            }
                        }

                        // The median is the center value.  This ignores the problem that occurs when weightSum is even,
                        // as it probably won't make a massive difference to the result and the solution is non-trivial.
                        var medianPosition = (weightSum / 2) + 1;

                        uint counter = 0;

                        // Count through the histogram until the median is found or passed.
                        for (var k = 0; k < 256; k++)
                        {
                            counter += histogram[k];
                            if (counter < medianPosition)
                            {
                                // We've not got to the median yet.  Keep counting.
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
