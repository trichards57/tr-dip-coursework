using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace ManagedDigitalImageProcessing.PGM
{
    class PgmImage
    {
        public PgmHeader Header { get; set; }

        public byte[] Data { get; set; }

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
