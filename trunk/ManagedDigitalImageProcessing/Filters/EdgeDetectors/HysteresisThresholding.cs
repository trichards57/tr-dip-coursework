using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    class HysteresisThresholding : FilterBase
    {
        private byte highT;
        private byte lowT;

        public HysteresisThresholding(byte highThreshold, byte lowThreshold)
        {
            highT = highThreshold;
            lowT = lowThreshold;
        }

        public override PGM.PgmImage Filter(PGM.PgmImage input)
        {
            bool[] output = new bool[input.Header.Height * input.Header.Width];

            var outImage = new PgmImage { Header = input.Header, Data = new byte[input.Header.Height * input.Header.Width] };

            Func<int, int, int> calculateIndex = (x, y) => CalculateIndex(x, y, input.Header.Width, input.Header.Height);

            int lastI, lastJ;

            for (var i = 0; i < input.Header.Width; i++)
            {
                for (var j = 0; j < input.Header.Height; j++)
                {
                    if (input.Data[calculateIndex(i, j)] > highT && !output[calculateIndex(i, j)])
                    {
                        output[calculateIndex(i, j)] = true;
                        output = Connect(i, j, output, input.Data, lowT, calculateIndex);
                    }
                    lastJ = j;
                }
                lastI = i;
            }

            var count = output.Count(t => t);

            for (var i = 0; i < output.Length; i++)
            {
                if (output[i])
                    outImage.Data[i] = 255;
                else
                    outImage.Data[i] = 0;
            }

            return outImage;
        }

        private bool[] Connect(int x, int y, bool[] output, byte[] input, byte lowThreshold, Func<int, int, int> calculateIndex)
        {
            for (int i = x - 1; i < x + 2; i++)
            {
                for (int j = y - 1; j < y + 2; j++)
                {
                    if (input[calculateIndex(i, j)] > lowThreshold && !output[calculateIndex(i, j)])
                    {
                        output[calculateIndex(i, j)] = true;
                        output = Connect(i, j, output, input, lowThreshold, calculateIndex);
                    }
                }
            }

            return output;
        }
    }
}
