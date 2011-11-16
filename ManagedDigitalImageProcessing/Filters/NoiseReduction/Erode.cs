// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Erode.cs" company="Tony Richards">
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
//   Defines the Erode type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    using System;
    using System.Threading.Tasks;

    using ManagedDigitalImageProcessing.Images;

    /// <summary>
    /// Filter class that applies the erode morphological operator to an image.
    /// </summary>
    public class Erode
    {
        /// <summary>
        /// The size of the structuring element
        /// </summary>
        private readonly int windowSize;

        /// <summary>
        /// <c>true</c> if the structuring element should be square
        /// </summary>
        private readonly bool square;

        /// <summary>
        /// Initializes a new instance of the <see cref="Erode"/> class.
        /// </summary>
        /// <param name="functionSize">Size of the function.</param>
        /// <param name="square">if set to <c>true</c>, the structuring object will be square.</param>
        public Erode(int functionSize = 3, bool square = false)
        {
            windowSize = functionSize;
            this.square = square;
        }

        /// <summary>
        /// Applies the operator to the specified image.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <returns>The eroded image.</returns>
        /// <remarks>
        /// Algorithm taken from @cite imageProcessingBook
        /// </remarks>
        public ImageData Filter(ImageData input)
        {
            // Partial function application to simplify index calculation.
            Func<int, int, int> calculateIndex = (x, y) => ImageUtilities.CalculateIndex(x, y, input.Width, input.Height);

            var output = new ImageData(input.Width, input.Height);

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
                    var min = int.MaxValue;

                    // Iterate through each of the items in the window, searching for the minimum value.
                    for (var k = -offset; k <= offset; k++)
                    {
                        for (var l = -offset; l <= offset; l++)
                        {
                            if (!this.square && Math.Sqrt((k * k) + (l * l)) >= offset)
                            {
                                continue;
                            }

                            var value = input.Data[calculateIndex(i + k, j + l)];
                            if (value < min)
                            {
                                min = value;
                            }
                        }
                    }
                    
                    // Set the output image's pixel value to the minimum value.
                    output.Data[calculateIndex(i, j)] = min;
                }
            });

            return output;
        }
    }
}
