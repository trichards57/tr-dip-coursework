using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;


namespace ManagedDigitalImageProcessing.Filters
{
    /// <summary>
    /// A base class to allow the CalculateIndex function to be reused, and to make calling multiple different
    /// filters in sequence easier.
    /// </summary>
    public abstract class FilterBase
    {
        /// <summary>
        /// Calculates the index of a point at given coordinates.  Points that fall out of the range of the
        /// image are reflected back in to the image (only works for positions less than width or height out
        /// of range).
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <returns>An index to access the given point in a 1D array.</returns>
        protected static int CalculateIndex(int x, int y, int width, int height)
        {
            if (x < 0)
                x = -x;
            if (x >= width)
                x = width - (x - width) - 1;

            if (y < 0)
                y = -y;
            if (y >= height)
                y = height - (y - height) - 1;

            return x + y * width;
        }

        protected static byte[] Convolve(int[] input, byte[] data, Size inputSize, Size dataSize)
        {
            Func<int, int, int> calculateInputIndex = (x, y) => CalculateIndex(x, y, inputSize.Width, inputSize.Height);
            Func<int, int, int> calculateDataIndex = (x, y) => CalculateIndex(x, y, dataSize.Width, dataSize.Height);

            var output = new int[data.Length];
            var xOffset = inputSize.Width / 2;
            var yOffset = inputSize.Height / 2;

            Parallel.For(0, dataSize.Width, i =>
            {
                for (var j = 0; j < dataSize.Height; j++)
                {
                    var sum = 0;
                    for (var k = 0; k < inputSize.Width; k++)
                    {
                        for (var l = 0; l < inputSize.Height; l++)
                        {
                            sum += data[calculateDataIndex(i + k - xOffset, j + l - yOffset)] * input[calculateInputIndex(k, l)];
                        }
                    }
                    output[calculateDataIndex(i, j)] = sum;
                }
            });

            var maxValue = output.AsParallel().Max();
            var minValue = output.AsParallel().Min();

            var byteOutput = new byte[data.Length];

            Parallel.For(0, output.Length, i =>
            {
                checked
                {
                    byteOutput[i] = (byte)((output[i] - minValue) * (255 / maxValue));
                }
            });

            return byteOutput;
        }

        protected static int[] Convolve(double[] input, int[] data, Size inputSize, Size dataSize)
        {
            Func<int, int, int> calculateInputIndex = (x, y) => CalculateIndex(x, y, inputSize.Width, inputSize.Height);
            Func<int, int, int> calculateDataIndex = (x, y) => CalculateIndex(x, y, dataSize.Width, dataSize.Height);

            var output = new int[data.Length];
            var xOffset = inputSize.Width / 2;
            var yOffset = inputSize.Height / 2;

            Parallel.For(0, dataSize.Width, i =>
                                                {
                                                    for (var j = 0; j < dataSize.Height; j++)
                                                    {
                                                        var sum = 0.0;
                                                        for (var k = 0; k < inputSize.Width; k++)
                                                        {
                                                            for (var l = 0; l < inputSize.Height; l++)
                                                            {
                                                                sum +=
                                                                    data[
                                                                        calculateDataIndex(i + k - xOffset,
                                                                                           j + l - yOffset)] *
                                                                    input[calculateInputIndex(k, l)];
                                                            }
                                                        }
                                                        output[calculateDataIndex(i, j)] = (int)Math.Floor(sum);
                                                    }
                                                });


            return output;
        }

        protected static byte[] Convolve(double[] input, byte[] data, Size inputSize, Size dataSize)
        {
            Func<int, int, int> calculateInputIndex = (x, y) => CalculateIndex(x, y, inputSize.Width, inputSize.Height);
            Func<int, int, int> calculateDataIndex = (x, y) => CalculateIndex(x, y, dataSize.Width, dataSize.Height);

            var output = new int[data.Length];
            var xOffset = inputSize.Width / 2;
            var yOffset = inputSize.Height / 2;

            Parallel.For(0, dataSize.Width, i =>
            {
                for (var j = 0; j < dataSize.Height; j++)
                {
                    double sum = 0;
                    for (var k = 0; k < inputSize.Width; k++)
                    {
                        for (var l = 0; l < inputSize.Height; l++)
                        {
                            sum += data[calculateDataIndex(i + k - xOffset, j + l - yOffset)] * input[calculateInputIndex(k, l)];
                        }
                    }
                    output[calculateDataIndex(i, j)] = (int)Math.Floor(sum);
                }
            });

            var maxValue = output.AsParallel().Max();
            var minValue = output.AsParallel().Min();

            var byteOutput = new byte[data.Length];

            if (minValue >= 0 && maxValue < 256)
            {
                for (var i = 0; i < output.Length;  i++)
                {
                    checked
                    {
                        byteOutput[i] = (byte)(output[i]);
                    }
                }
            }
            else
            {
                for (var i = 0; i < output.Length; i++)
                {
                    checked
                    {
                        byteOutput[i] = (byte)((output[i]) * (255 / maxValue));
                    }
                }
            }

            return byteOutput;
        }
    }
}
