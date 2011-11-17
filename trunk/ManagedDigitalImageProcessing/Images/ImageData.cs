// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageData.cs" company="Tony Richards">
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
//   An image loaded from a file and kept in a 1D array.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.Images
{
    using System;
    using System.Drawing;
    using System.Linq;

    /// <summary>
    /// An image loaded from a file and kept in a 1D array.
    /// </summary>
    public sealed class ImageData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageData"/> class.
        /// </summary>
        public ImageData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageData"/> class.
        /// </summary>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        public ImageData(int width, int height)
        {
            Width = width;
            Height = height;
            Data = new int[width * height];
        }

        /// <summary>
        /// Gets or sets the width of the image.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the image.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the image data.
        /// </summary>
        /// <value>
        /// The image data.
        /// </value>
        public int[] Data { get; set; }

        /// <summary>
        /// Converts the image to a Bitmap.
        /// </summary>
        /// <returns>The image, rendered as a bitmap</returns>
        public Bitmap ToBitmap()
        {
            var output = new Bitmap(Width, Height);

            var maxElement = Data.AsParallel().Max();
            var minElement = Data.AsParallel().Min();

            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    // We've been working with int values until now.
                    // Normalise and scale them so that the full range is shown in the image.
                    double val = Data[ImageUtilities.CalculateIndex(i, j, Width)];
                    val -= minElement;
                    if (maxElement != minElement)
                    {
                        val *= 255.0 / (maxElement - minElement);
                    }

                    // This shouldn't ever throw an exception, but this will prevent odd images being produced if the integer overflows.
                    checked
                    {
                        var pixValue = (byte)Math.Floor(val);
                        output.SetPixel(i, j, Color.FromArgb(pixValue, pixValue, pixValue));
                    }  
                }
            }

            return output;
        }
    }
}
