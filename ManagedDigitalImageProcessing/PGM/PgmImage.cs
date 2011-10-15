using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace ManagedDigitalImageProcessing.PGM
{
    /// <summary>
    /// An image loaded from a %PGM file.
    /// </summary>
    class PgmImage
    {
        /// <summary>
        /// Gets or sets the header of the file.
        /// </summary>
        /// <value>
        /// The header of the file.
        /// </value>
        public PgmHeader Header { get; set; }

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
        /// <returns></returns>
        public Bitmap ToBitmap()
        {
            var output = new Bitmap(Header.Width, Header.Height);

            for (var i = 0; i < Header.Height; i++)
            {
                for (var j = 0; j < Header.Width; j++)
                {
                    output.SetPixel(j, i, Color.FromArgb(Data[i * Header.Width + j], Data[i * Header.Width + j], Data[i * Header.Width + j]));
                }
            }

            return output;
        }
    }
}
