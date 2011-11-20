// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MorphologicalGradient.cs" company="Tony Richards">
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
//   Defines the MorphologicalGradient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    using System.Linq;
    using Images;
    using NoiseReduction;

    /// <summary>
    /// Filter class to apply a morpholgical gradient operation to an image, highlighting the edges.
    /// </summary>
    public static class MorphologicalGradient
    {
        /// <summary>
        /// Filters the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="erodeElementSize">Size of the erode.</param>
        /// <param name="dilateElementSize">Size of the dilate.</param>
        /// <returns>
        /// An image with the edges highlighted by the morphological gradient operator.
        /// </returns>
        /// <remarks>
        /// Algorithm taken from @cite imageProcessingBook
        /// </remarks>
        public static ImageData Filter(ImageData image, int erodeElementSize, int dilateElementSize)
        {
            // First create two images, one an erosion of the original and one a dilation
            var er = Erode.Filter(image, erodeElementSize);
            var di = Dilate.Filter(image, dilateElementSize);

            // Now take the differences of the two images to create a new image
            var buffer =
                di.Data.AsParallel().AsOrdered().Zip(er.Data.AsParallel().AsOrdered(), (d, e) => d - e).ToArray();

            return new ImageData { Width = image.Width, Height = image.Height, Data = buffer };
        }

        /// <summary>
        /// Filters the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="structuringElementSize">Size of the structuring element.</param>
        /// <returns>An image with the edges highlighted by the morphological gradient operator.</returns>
        public static ImageData Filter(ImageData image, int structuringElementSize)
        {
            return Filter(image, structuringElementSize, structuringElementSize);
        }
    }
}
