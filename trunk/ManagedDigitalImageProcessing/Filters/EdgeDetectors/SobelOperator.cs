// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SobelOperator.cs" company="Tony Richards">
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
//   Defines the SobelOperator type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ManagedDigitalImageProcessing.Images;

    /// <summary>
    /// Filter class, used to run the Sobel operator on an image.
    /// </summary>
    public sealed class SobelOperator
    {
        /// <summary>
        /// Applies the Sobel operator to the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The filtered image.</returns>
        public ImageData Filter(ImageData input)
        {
            var output = new ImageData(input.Width, input.Height);

            var outputTemp = FilterSplit(input);

            // Transform, in parallel, the calculated vectors in to their magnitudes
            Parallel.For(
                0,
                input.Data.Length,
                i =>
                output.Data[i] = (int)Math.Round(Math.Sqrt((outputTemp.XData[i] * outputTemp.XData[i]) + (outputTemp.YData[i] * outputTemp.YData[i]))));

            return output;
        }

        /// <summary>
        /// Applies the Sobel operator to the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The edge vectors from the filtered image.</returns>
        public SobelOperatorResult FilterSplit(ImageData input)
        {
            var output = new SobelOperatorResult { Width = input.Width, Height = input.Height };

            // Define the templates.
            var template1 = new[] { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
            var template2 = new[] { 1, 2, 1, 0, 0, 0, -1, -2, -1 };

            var intData = input.Data;

            // Convolve the first template across the image, using the absolute value to ensure edges in both direction are treated equally
            var data = ImageUtilities.Convolve(template1, intData, new System.Drawing.Size(3, 3), new System.Drawing.Size(input.Width, input.Height));
            output.XData = data.AsParallel().Select(Math.Abs).ToArray();

            // Do the same again for the second template.
            data = ImageUtilities.Convolve(template2, intData, new System.Drawing.Size(3, 3), new System.Drawing.Size(input.Width, input.Height));
            output.YData = data.AsParallel().Select(Math.Abs).ToArray();

            return output;
        }
    }
}
