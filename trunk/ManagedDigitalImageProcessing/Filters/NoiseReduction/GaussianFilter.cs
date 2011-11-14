using System;
using System.Drawing;
using System.Threading.Tasks;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    public sealed class GaussianFilter : FilterBase
    {
        private readonly double[] _template;
        private readonly int _size;
        private readonly double _sigma;

        public GaussianFilter(double sigma)
            : this((int)(2 * Math.Ceiling(3 * sigma) + 1), sigma)
        {
            _sigma = sigma;
        }

        public GaussianFilter(int size, double sigma)
        {
            _size = size;
            var templateTemp = new double[size * size];
            double sum = 0;

            Func<int, int, int> calculateIndex = (x, y) => CalculateIndex(x, y, size, size);

            var centre = size / 2;

            Parallel.For(0, size, i =>
                                      {
                                          for (var j = 0; j < size; j++)
                                          {
                                              templateTemp[calculateIndex(i, j)] =
                                                  Math.Exp(-(((j - centre) * (j - centre)) + ((i - centre) * (i - centre))) /
                                                           (2 * sigma * sigma));
                                              sum += templateTemp[calculateIndex(i, j)];
                                          }
                                      });

            _template = new double[templateTemp.Length];
            Parallel.For(0, templateTemp.Length, i => _template[i] = templateTemp[i] / sum);
        }

        public PgmImage Filter(PgmImage input)
        {
            var output = new PgmImage { Header = input.Header, Data = new byte[input.Data.Length] };

            output.Data = Convolve(_template, input.Data, new Size(_size, _size), new Size(input.Header.Width, input.Header.Height));

            return output;
        }

        public override string ToString()
        {
            return string.Format("Gauss {0}", _sigma);
        }


        public int Size { get { return _size; } }

        public double Sigma { get { return _sigma; } }
    }
}
