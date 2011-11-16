// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PgmImage.cs" company="Tony Richards">
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
//   An image loaded from a %PGM file.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.Images
{
    using System.Drawing;

    /// <summary>
    /// An image loaded from a file and kept in a 1D array.
    /// </summary>
    public sealed class ImageData
    {
        /// <summary>
        /// Gets or sets the width of the image.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width
        {
            get
            {
                return Header.Width;
            }

            set
            {
                Header.Width = value;
            }
        }

        /// <summary>
        /// Gets or sets the height of the image.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height
        {
            get
            {
                return Header.Height;
            }

            set
            {
                Header.Height = value;
            }
        }

        /// <summary>
        /// Gets or sets the header of the file.
        /// </summary>
        /// <value>
        /// The header of the file.
        /// </value>
        private ImageHeader Header { get; set; }

        /// <summary>
        /// Gets or sets the image data.
        /// </summary>
        /// <value>
        /// The image data.
        /// </value>
        public byte[] Data { get; set; }

        /// <summary>
        /// Converts the image to a Bitmap.
        /// </summary>
        /// <returns>The image, rendered as a bitmap</returns>
        public Bitmap ToBitmap()
        {
            var output = new Bitmap(Header.Width, Header.Height);

            for (var i = 0; i < Header.Height; i++)
            {
                for (var j = 0; j < Header.Width; j++)
                {
                    output.SetPixel(j, i, Color.FromArgb(Data[(i * Header.Width) + j], Data[(i * Header.Width) + j], Data[(i * Header.Width) + j]));
                }
            }

            return output;
        }
    }
}
