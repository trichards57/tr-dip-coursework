﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Close.cs" company="Tony Richards">
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
//   Defines the Close type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    using ManagedDigitalImageProcessing.Images;

    /// <summary>
    /// A filter class to apply the close morphological operator to an image.
    /// </summary>
    public static class Close
    {
        /// <summary>
        /// Applies the close operator to the input image.
        /// </summary>
        /// <param name="image">The input image.</param>
        /// <param name="erodeSize">Size of the erode structuring element.</param>
        /// <param name="dilateSize">Size of the dilate structuring element.</param>
        /// <returns>
        /// The closed image
        /// </returns>
        /// <remarks>
        /// Algorithm taken from @cite imageProcessingBook
        /// </remarks>
        public static ImageData Filter(ImageData image, int erodeSize, int dilateSize)
        {
            // Closing is a dilation followed by an erosion
            return Erode.Filter(Dilate.Filter(image, dilateSize), erodeSize);
        }

        /// <summary>
        /// Filters the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="structuringElementSize">Size of the structuring elements.</param>
        /// <returns>
        /// The closed image
        /// </returns>
        public static ImageData Filter(ImageData image, int structuringElementSize)
        {
            return Filter(image, structuringElementSize, structuringElementSize);
        }
    }
}
