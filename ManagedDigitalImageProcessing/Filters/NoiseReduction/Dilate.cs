// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Dilate.cs" company="Tony Richards">
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
//   Defines the Dilate type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    using System;
    using System.Threading.Tasks;

    using ManagedDigitalImageProcessing.Images;

    /// <summary>
    /// Filer class that applies the dilate morphological operator to an image.
    /// </summary>
    public static class Dilate
    {
        /// <summary>
        /// Applies the operator to the specified input.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <param name="structuringElementSize">Size of the structuring element.</param>
        /// <param name="square">if set to <c>true</c>, the structuring element will be square.</param>
        /// <returns>
        /// The dilated image.
        /// </returns>
        /// <remarks>
        /// Algorithm taken from @cite imageProcessingBook
        /// </remarks>
        public static ImageData Filter(ImageData input, int structuringElementSize = 3, bool square = false)
        {
            var output = new ImageData(input.Width, input.Height);

            // Partial function application to simplify index calculation.
            Func<int, int, int> calculateIndex = (x, y) => ImageUtilities.CalculateIndex(x, y, input.Width, input.Height);
            Func<int, int, int> calculateOutputIndex = (x, y) => ImageUtilities.CalculateIndex(x, y, output.Width);

            var offset = structuringElementSize / 2;

            // Iterate through each column.
            Parallel.For(
                0,
                output.Width,
                i =>
                {
                    // Iterate through each point in the column.
                    for (var j = 0; j < output.Height; j++)
                    {
                        var max = int.MinValue;

                        // Iterate through each of the items in the window, searching for the maximum value
                        for (var k = -offset; k <= offset; k++)
                        {
                            for (var l = -offset; l <= offset; l++)
                            {
                                if (!square && Math.Sqrt((k * k) + (l * l)) >= offset)
                                {
                                    continue;
                                }

                                var value = input.Data[calculateIndex(i + k, j + l)];
                                if (value > max)
                                {
                                    max = value;
                                }
                            }
                        }

                        // Set the output image's pixel value to the maximum value.
                        output.Data[calculateOutputIndex(i, j)] = max;
                    }
                });

            return output;
        }
    }
}
