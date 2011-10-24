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
        public byte[] Peak { get; set; }

        public PgmImage ToPgmImage()
        {
            var output = new PgmImage { Data = new byte[Header.Height * Header.Width], Header = Header };

            for (var i = 0; i < Header.Width; i++)
            {
                for (var j = 0; j < Header.Height; j++)
                {
                    if (Peak[i + j * Header.Width] > 0)
                        output.Data[i + j * Header.Width] = Peak[i + j * Header.Width];
                    else
                        output.Data[i + j * Header.Width] = 0;
                }
            }

            return output;
        }

        public Bitmap ToBitmap()
        {
            return ToPgmImage().ToBitmap();
        }
    }
}
