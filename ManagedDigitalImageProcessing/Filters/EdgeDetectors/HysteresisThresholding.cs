using System;
using System.Collections.Generic;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    public sealed class HysteresisThresholding : FilterBase
    {
        private readonly byte _highT;
        private readonly byte _lowT;

        public HysteresisThresholding(byte highThreshold, byte lowThreshold)
        {
            _highT = highThreshold;
            _lowT = lowThreshold;
        }

        public override PgmImage Filter(PgmImage input)
        {
            var output = new bool[input.Header.Height * input.Header.Width];

            var outImage = new PgmImage { Header = input.Header, Data = new byte[input.Header.Height * input.Header.Width] };

            Func<int, int, int> calculateIndex = (x, y) => CalculateIndex(x, y, input.Header.Width, input.Header.Height);

            for (var i = 0; i < input.Header.Width; i++)
            {
                for (var j = 0; j < input.Header.Height; j++)
                {
                    if (input.Data[calculateIndex(i, j)] <= _highT || output[calculateIndex(i, j)]) continue;
                    output[calculateIndex(i, j)] = true;
                    output = Connect(i, j, output, input.Data, _lowT, calculateIndex);
                }
            }

            for (var i = 0; i < output.Length; i++)
            {
                if (output[i])
                    outImage.Data[i] = 255;
                else
                    outImage.Data[i] = 0;
            }

            return outImage;
        }

        private static bool[] Connect(int x, int y, bool[] output, IList<byte> input, byte lowThreshold, Func<int, int, int> calculateIndex)
        {
            for (var i = x - 1; i < x + 2; i++)
            {
                for (var j = y - 1; j < y + 2; j++)
                {
                    if (input[calculateIndex(i, j)] <= lowThreshold || output[calculateIndex(i, j)]) continue;
                    output[calculateIndex(i, j)] = true;
                    output = Connect(i, j, output, input, lowThreshold, calculateIndex);
                }
            }

            return output;
        }
    }
}
