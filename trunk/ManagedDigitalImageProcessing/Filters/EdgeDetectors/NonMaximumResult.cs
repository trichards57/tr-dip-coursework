// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NonMaximumResult.cs" company="Tony Richards">
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
//   Defines the NonMaximumResult type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    using System.Drawing;
    using System.Threading.Tasks;

    using ManagedDigitalImageProcessing.Images;

    /// <summary>
    /// A class to hold the results of the <see cref="NonMaximumSuppression.Filter"/> function.
    /// </summary>
    public sealed class NonMaximumResult
    {
        /// <summary>
        /// Gets or sets the width of the image.
        /// </summary>
        /// <value>
        /// The width of the image.
        /// </value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the image.
        /// </summary>
        /// <value>
        /// The height of the image.
        /// </value>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the X data.
        /// </summary>
        /// <value>
        /// The X data.
        /// </value>
        public byte[] XData { get; set; }

        /// <summary>
        /// Gets or sets the Y data.
        /// </summary>
        /// <value>
        /// The Y data.
        /// </value>
        public byte[] YData { get; set; }

        /// <summary>
        /// Gets or sets the non-maximum suppressed values.
        /// </summary>
        /// <value>
        /// The peak.
        /// </value>
        public byte[] Peak { get; set; }

        /// <summary>
        /// Converts this class to a PgmImage (allowing it to be converted to a bitmap easily).
        /// </summary>
        /// <returns>A PgmImage representing this result</returns>
        public ImageData ToPgmImage()
        {
            var output = new ImageData(Width, Height);

            Parallel.For(
                0, 
                Width,
                i =>
            {
                for (var j = 0; j < Height; j++)
                {
                    if (Peak[i + (j * Width)] > 0)
                    {
                        output.Data[i + (j * Width)] = Peak[i + (j * Width)];
                    }
                    else
                    {
                        output.Data[i + (j * Width)] = 0;
                    }
                }
            });

            return output;
        }

        /// <summary>
        /// Converts this to a Bitmap image for display.
        /// </summary>
        /// <returns>A Bitmap image representing this result</returns>
        /// <remarks>
        /// Converts the result to an image via the <see cref="ToPgmImage"/> function.
        /// </remarks>
        public Bitmap ToBitmap()
        {
            return ToPgmImage().ToBitmap();
        }
    }
}
