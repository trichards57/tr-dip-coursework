// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MedianFilter.cs" company="Tony Richards">
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
//   Apply a median filter to the input.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    using System;
    using System.Threading.Tasks;

    using ManagedDigitalImageProcessing.Images;

    /// <summary>
    /// Filter class to apply a median filter to the input.
    /// </summary>
    public static class MedianFilter
    {
        /// <summary>
        /// Filters the specified input.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <param name="windowSize">Size of the filter window.</param>
        /// <returns>
        /// The filtered image.
        /// </returns>
        public static ImageData Filter(ImageData input, int windowSize = 3)
        {
            if (windowSize % 2 == 0)
            {
                throw new ArgumentOutOfRangeException("windowSize", windowSize, "The size must be odd.");
            }

            var output = new ImageData(input.Width, input.Height);

            // Partial function application to simplify index calculation.
            Func<int, int, int> calculateOutputIndex = (x, y) => ImageUtilities.CalculateIndex(x, y, output.Width);
            Func<int, int, int> calculateIndex = (x, y) => ImageUtilities.CalculateIndex(x, y, input.Width, input.Height);

            var offset = windowSize / 2;

            // Iterate through each column.
            Parallel.For(
                0,
                output.Width,
                i =>
                {
                    // Iterate through each point in the column.
                    for (var j = 0; j < output.Height; j++)
                    {
                        var list = new int[windowSize * windowSize];
                        var count = 0;

                        // Iterate through each of the items in the window, adding the value to a list.
                        for (var k = -offset; k <= offset; k++)
                        {
                            for (var l = -offset; l <= offset; l++)
                            {
                                list[count++] = input.Data[calculateIndex(i + k, j + l)];
                            }
                        }

                        // Sort the list.
                        Array.Sort(list);

                        // Take the middle value (the median) and insert it in to the new image.
                        output.Data[calculateOutputIndex(i, j)] = list[list.Length / 2];
                    }
                });

            return output;
        }
    }
}
