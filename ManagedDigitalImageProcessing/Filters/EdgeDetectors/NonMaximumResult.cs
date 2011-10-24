using System;
using System.Drawing;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    public sealed class NonMaximumResult
    {
        public PgmHeader Header { get; set; }
        public byte[] XData { get; set; }
        public byte[] YData { get; set; }
        public bool[] Peak { get; set; }

        public PgmImage ToPgmImage()
        {
            var output = new PgmImage { Data = new byte[Header.Height * Header.Width], Header = Header };

            for (var i = 0; i < Header.Width; i++)
            {
                for (var j = 0; j < Header.Height; j++)
                {
                    if (Peak[i + j * Header.Width])
                        output.Data[i + j * Header.Width] = (byte)Math.Sqrt(XData[i + j * Header.Width] * XData[i + j * Header.Width] + YData[i + j * Header.Width] * YData[i + j * Header.Width]);
                    else
                        output.Data[i + j * Header.Width] = 0;
                }
            }

            return output;
        }

        public Bitmap ToBitmap()
        {
            var output = new Bitmap(Header.Width, Header.Height);

            for (var i = 0; i < Header.Width; i++)
            {
                for (var j = 0; j < Header.Height; j++)
                {
                    if (Peak[i + j * Header.Width])
                        output.SetPixel(i, j, Color.White);
                    else
                        output.SetPixel(i, j, Color.Black);
                }
            }

            return output;
        }
    }
}
