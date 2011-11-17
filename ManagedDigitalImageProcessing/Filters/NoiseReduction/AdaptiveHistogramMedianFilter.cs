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

    using ManagedDigitalImageProcessing.Images;

    /// <summary>
    /// Apply an adaptive median filter to the input, using a histogram to optimise the median operation.
    /// </summary>
    /// From 'An Adaptive Weighted Median Filter for Speckle Suppression in Medical Ultrasonic Images'
    public sealed class AdaptiveHistogramMedianFilter
    {
        /// <summary>
        /// The size of the filter window.
        /// </summary>
        private readonly int windowSize;

        /// <summary>
        /// The value of the center weight of the filter.
        /// </summary>
        private readonly double centreWeight;

        /// <summary>
        /// The value of the scaling constant used to determine the filter weights.
        /// </summary>
        private readonly double scalingConstant;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdaptiveHistogramMedianFilter"/> class.
        /// </summary>
        /// <param name="centreWeight">The centre weight for the filter.</param>
        /// <param name="scalingConstant">The scaling constant used to determine the filter weights.</param>
        /// <param name="size">The size of the filter window.</param>
        public AdaptiveHistogramMedianFilter(double centreWeight, double scalingConstant, int size = 3)
        {
            if (size % 2 == 0)
            {
                throw new ArgumentOutOfRangeException("size", size, "The size must be odd.");
            }

            windowSize = size;
            this.centreWeight = centreWeight;
            this.scalingConstant = scalingConstant;
        }

        /// <summary>
        /// Filters the specified input.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <returns>
        /// The filtered image.
        /// </returns>
        public ImageData Filter(ImageData input)
        {
            var output = new ImageData(input.Width, input.Height);

            // Convenience function to simplify index calculation.
            Func<int, int, int> calculateIndex = (x, y) => ImageUtilities.CalculateIndex(x, y, input.Width, input.Height);
            Func<int, int, int> calculateOutputIndex = (x, y) => ImageUtilities.CalculateIndex(x, y, output.Width);

            var offset = windowSize / 2;
            var itemCount = windowSize * windowSize;

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
                        var histogram = new uint[256];

                        for (var k = -offset; k <= offset; k++)
                        {
                            for (var l = -offset; l <= offset; l++)
                            {
                                var level = input.Data[calculateIndex(i + k, j + l)];
                                sum += level;
                            }
                        }

                        uint counter = 0;

                        var mean = sum / itemCount;
                        var squareDiffTotal = 0.0;

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

                        for (var k = -offset; k <= offset; k++)
                        {
                            for (var l = -offset; l <= offset; l++)
                            {
                                var level = input.Data[calculateIndex(i + k, j + l)];
                                var distance = Math.Sqrt((k * k) + (l * l));
                                var weight = centreWeight - (scalingConstant * distance * variance / mean);
                                weight = weight < 0 ? 0 : Math.Round(weight);
                                histogram[level] += (uint)weight;
                                weightSum += (uint)weight;
                            }
                        }

                        var medianPosition = (weightSum / 2) + 1;

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
