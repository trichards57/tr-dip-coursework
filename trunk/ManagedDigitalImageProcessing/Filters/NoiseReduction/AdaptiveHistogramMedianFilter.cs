using System;
using System.Drawing;
using System.Threading.Tasks;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    /// <summary>
    /// Apply an adaptive median filter to the input, using a histogram to optimise the median operation.
    /// </summary>
    /// 
    /// From 'An Adaptive Weighted Median Filter for Speckle Suppression in Medical Ultrasonic Images'
    public sealed class AdaptiveHistogramMedianFilter : FilterBase
    {
        /// <summary>
        /// The size of the filter window.
        /// </summary>
        private readonly int _windowSize;

        private double _centreWeight;
        private double _scalingConstant;


        /// <summary>
        /// Initializes a new instance of the <see cref="HistogramMedianFilter"/> class.
        /// </summary>
        /// <param name="centreWeight">The centre weight.</param>
        /// <param name="scalingConstant">The scaling constant.</param>
        /// <param name="size">The filter window size.</param>
        public AdaptiveHistogramMedianFilter(double centreWeight, double scalingConstant, int size = 3)
        {
            if (size % 2 == 0)
                throw new ArgumentOutOfRangeException("size", size, "The size must be odd.");

            _windowSize = size;
            _centreWeight = centreWeight;
            _scalingConstant = scalingConstant;

        }

        /// <summary>
        /// Filters the specified input.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <returns>
        /// The filtered image.
        /// </returns>
        public PgmImage Filter(PgmImage input)
        {
            var output = new PgmImage { Header = input.Header, Data = new byte[input.Data.Length] };

            ;
            // Partial function application to simplify index calculation.
            Func<int, int, int> calculateIndex = ((x, y) => CalculateIndex(x, y, input.Header.Width, input.Header.Height));

            var offset = _windowSize / 2;
            var itemCount = _windowSize * _windowSize;

            // Iterate through each column.
            Parallel.For(0, output.Header.Width, i =>
                                                     {
                                                         // Iterate through each point in the column.
                                                         for (var j = 0; j < output.Header.Height; j++)
                                                         {
                                                             var sum = 0L;
                                                             var histogram = new uint[256];

                                                             for (var k = -offset; k <= offset; k++)
                                                             {
                                                                 for (var l = -offset; l <= offset; l++)
                                                                 {
                                                                     var level =
                                                                         input.Data[calculateIndex(i + k, j + l)];
                                                                     sum += level;
                                                                 }
                                                             }


                                                             uint counter = 0;

                                                             var mean = (double)sum / itemCount;
                                                             var squareDiffTotal = 0.0;

                                                             for (var k = -offset; k <= offset; k++)
                                                             {
                                                                 for (var l = -offset; l <= offset; l++)
                                                                 {
                                                                     var level =
                                                                         input.Data[calculateIndex(i + k, j + l)];
                                                                     var diff = level - mean;
                                                                     squareDiffTotal += diff * diff;
                                                                 }
                                                             }

                                                             var variance = squareDiffTotal / itemCount;
                                                             uint weightSum = 0;

                                                             for (var k = -offset; k <= offset; k++)
                                                             {
                                                                 for (var l = -offset; l <= offset; l++)
                                                                 {
                                                                     var level =
                                                                         input.Data[calculateIndex(i + k, j + l)];
                                                                     var distance = Math.Sqrt(k * k + l * l);
                                                                     var weight = _centreWeight -
                                                                                  _scalingConstant * distance * variance /
                                                                                  mean;
                                                                     if (weight < 0)
                                                                         weight = 0;
                                                                     else
                                                                         weight = Math.Round(weight);
                                                                     histogram[level] += (uint)weight;
                                                                     weightSum += (uint)weight;
                                                                 }
                                                             }

                                                             var medianPosition = ((weightSum) / 2) + 1;

                                                             if (medianPosition > 1)
                                                                 medianPosition = medianPosition;

                                                             // Count through the histogram until the median is found or passed.
                                                             for (var k = 0; k < 256; k++)
                                                             {
                                                                 counter += histogram[k];
                                                                 if (counter < medianPosition) continue;
                                                                 // We've reached the histogram item that contains the median value.
                                                                 // Return it.
                                                                 output.Data[calculateIndex(i, j)] = (byte)k;
                                                                 break;
                                                             }
                                                         }
                                                     });

            return output;
        }


        public override string ToString()
        {
            return string.Format("Histogram {0}", _windowSize);
        }
    }
}
