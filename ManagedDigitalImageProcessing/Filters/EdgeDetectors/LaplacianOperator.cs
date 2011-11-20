// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LaplacianOperator.cs" company="Tony Richards">
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
//   Defines the LaplacianOperator type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    using ManagedDigitalImageProcessing.Images;

    /// <summary>
    /// A filter object to apply the Laplacian Operator to an image.
    /// </summary>
    public static class LaplacianOperator
    {
        /// <summary>
        /// Applies the Laplacian Operator to the given image.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <returns>The processed image</returns>
        /// <remarks>
        /// Algorithm taken from @cite imageProcessingBook
        /// </remarks>
        public static ImageData Filter(ImageData input)
        {
            var output = new ImageData(input.Width, input.Height);

            var template = new[] { 0, -1, 0, -1, 4, -1, 0, -1, 0 };

            output.Data = ImageUtilities.Convolve(template, input.Data, new System.Drawing.Size(3, 3), new System.Drawing.Size(input.Width, input.Height));

            return output;
        }
    }
}
