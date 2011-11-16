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
    using ManagedDigitalImageProcessing.Filters.NoiseReduction;
    using ManagedDigitalImageProcessing.PGM;

    /// <summary>
    /// Filter class to apply a morpholgical gradient operation to an image, highlighting the edges.
    /// </summary>
    internal class MorphologicalGradient
    {
        /// <summary>
        /// The size of the erosion operator structuring element.
        /// </summary>
        private readonly int erodeSize;

        /// <summary>
        /// The size of the dilation operator structuring element.
        /// </summary>
        private readonly int dilateSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="MorphologicalGradient"/> class.
        /// </summary>
        /// <param name="erodeSize">Size of the erosion structuring element.</param>
        /// <param name="dilateSize">Size of the dilation structuring element.</param>
        public MorphologicalGradient(int erodeSize, int dilateSize)
        {
            this.erodeSize = erodeSize;
            this.dilateSize = dilateSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MorphologicalGradient"/> class.
        /// </summary>
        /// <param name="filterSize">Size of the both of the structuring elements.</param>
        public MorphologicalGradient(int filterSize)
            : this(filterSize, filterSize)
        {
        }

        /// <summary>
        /// Filters the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>An image with the edges highlighted by the morphological gradient operator.</returns>
        /// <remarks>
        /// Algorithm taken from @cite imageProcessingBook
        /// </remarks>
        public PgmImage Filter(PgmImage image)
        {
            var dilateFilter = new Dilate(dilateSize);
            var erodeFilter = new Erode(erodeSize);

            // First create two images, one an erosion of the original and one a dilation
            var er = erodeFilter.Filter(image);
            var di = dilateFilter.Filter(image);

            var output = new PgmImage { Header = image.Header, Data = new byte[er.Data.Length] };

            var buffer = new int[er.Data.Length];
            var max = 0;

            // Now take the differences of the two images to create a new image
            for (var i = 0; i < er.Data.Length; i++)
            {
                var val = di.Data[i] - er.Data[i];

                // Constrain value to greater than 0
                if (val < 0)
                {
                    val = 0;
                }

                buffer[i] = val;
                if (val > max)
                {
                    max = val;
                }
            }

            // Normalise the results to ensure they fit in to a byte array
            for (var i = 0; i < er.Data.Length; i++)
            {
                var scale = 255.0 / max;

                output.Data[i] = (byte)(scale * buffer[i]);
            }

            return output;
        }
    }
}
