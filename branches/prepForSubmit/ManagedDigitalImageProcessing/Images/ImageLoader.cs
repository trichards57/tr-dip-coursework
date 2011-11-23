// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageLoader.cs" company="Tony Richards">
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
//   Loads pixel data out of an image file.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.Images
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;

    using Exceptions;

    /// <summary>
    /// Loads pixel data out of an image file.
    /// </summary>
    public static class ImageLoader
    {
        /// <summary>
        /// Loads a PGM image.
        /// </summary>
        /// <param name="instream">The stream to load the image from.</param>
        /// <returns>A PgmImage containing the loaded image.</returns>
        /// <exception cref="InvalidPgmHeaderException">
        /// Thrown if there isn't enough data in the file to fill the image defined in the  header.
        /// </exception>
        /// <remarks>
        /// Not particularly safe.  Falls down if the entire file is long enough to fill the image but the actual data section 
        /// (i.e. the section after the header) isn't (it will load the header in to the image).
        /// </remarks>
        public static ImageData LoadPgmImage(Stream instream)
        {
            var reader = new StreamReader(instream);
            // Read the header out of the image.
            var header = ReadPGMHeader(reader);

            var length = header.Height * header.Width;

            // Reposition the stream curser (StreamReaders cache data, meaning the current stream position is not
            // necessarily the end of the header).
            // This is not foolproof, as it could position the stream in the middle of the header if the length is wrong, 
            // causing the header to be loaded as part of the image.
            instream.Seek(-length, SeekOrigin.End);
            var binaryReader = new BinaryReader(instream);

            // Read the raw binary data a byte at a time, storing each byte in an integer value (to allow room for later).
            var data = binaryReader.ReadBytes(length).Select(t => (int)t).ToArray();

            // If the file is too short, not enough pixels will have been loaded.
            if (data.Length < header.Height * header.Width)
            {
                throw new InvalidPgmHeaderException(
                    string.Format("Not enough data in the file. {0} bytes read.", data.Length));
            }

            // Wrap the data up in a class that describes it.
            return new ImageData { Data = data, Height = header.Height, Width = header.Width };
        }

        /// <summary>
        /// Converts a .Net Bitmap to an ImageData, giving easier access to the pixel data.
        /// </summary>
        /// <param name="file">The bitmap to read.</param>
        /// <returns>A PGMImage representation of the bitmap</returns>
        /// <remarks>This only works with grayscale images. Running is on colour images could produce wierd results.</remarks>
        public static ImageData LoadBitmap(Bitmap file)
        {
            // Pin the image in memory to allow the image data to be accessed, all in one go.
            var info = file.LockBits(new Rectangle(0, 0, file.Width, file.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, file.PixelFormat);
            // Calculate how much room we'll need.
            var bytes = Math.Abs(info.Stride) * file.Height;
            var values = new byte[bytes];

            // Load the pixel data in to the array.  This *will* go wrong if the image isn't an 8-bit grayscale image.
            Marshal.Copy(info.Scan0, values, 0, bytes);
            // Unlock the image and release any unmanaged resources used to lock them.
            file.UnlockBits(info);

            // Wrap the data up in a class that describes it.
            return new ImageData { Data = values.Select(t => (int)t).ToArray(), Height = file.Height, Width = info.Stride };
        }

        /// <summary>
        /// Reads the header from a PGM image.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <returns>A PgmHeader loaded from <paramref name="reader"/>.</returns>
        /// <remarks>
        /// Falls down when the header contains a comment.
        /// </remarks>
        private static ImageHeader ReadPGMHeader(TextReader reader)
        {
            var header = new ImageHeader();
            var line = reader.ReadLine();

            // Check that this is the right sort of PGM image.  Can't currently handle any other format.
            if (line != "P5")
            {
                throw new InvalidPgmHeaderException(string.Format("First line was not 'P5' : {0}", line));
            }

            // I've assumed that the input file is valid.  This will cause exceptions if the file is empty.
            line = reader.ReadLine();

            var parts = line.Split(new[] { ' ', '\r', '\n' });

            int col, row, maxGrey;

            // Extract out the image size.
            if (!(int.TryParse(parts[0], out col) && int.TryParse(parts[1], out row)))
            {
                throw new InvalidPgmHeaderException(
                    string.Format("Couldn't read column and row definition : {0}", line));
            }

            header.Width = col;
            header.Height = row;

            // Get the next line and read out the maximum gray value.
            line = reader.ReadLine();

            if (!int.TryParse(line, out maxGrey))
            {
                throw new InvalidPgmHeaderException(string.Format("Couldn't read maximum grey value : {0}", line));
            }

            if (maxGrey > 256)
            {
                // The file loading code later can't handle files where each pixel takes up more than 8 bits.
                throw new InvalidPgmHeaderException(string.Format("Maximum grey value too large : {0}", maxGrey));
            }

            return header;
        }

        /// <summary>
        /// The header of a image.
        /// </summary>
        private sealed class ImageHeader
        {
            /// <summary>
            /// Gets or sets the height of the file.
            /// </summary>
            /// <value>
            /// The height.
            /// </value>
            public int Height { get; set; }

            /// <summary>
            /// Gets or sets the width of the file.
            /// </summary>
            /// <value>
            /// The width.
            /// </value>
            public int Width { get; set; }
        }
    }
}
