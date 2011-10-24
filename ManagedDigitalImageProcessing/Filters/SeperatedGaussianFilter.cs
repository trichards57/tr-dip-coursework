using System;
using System.Drawing;
using System.Linq;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters
{
    public sealed class SeperatedGaussianFilter : FilterBase
    {
        private readonly double[] _template;
        private readonly int _size;
        private readonly double _sigma;

        public SeperatedGaussianFilter(double sigma)
            : this((int)(2 * Math.Ceiling(3 * sigma) + 1), sigma)
        {
            _sigma = sigma;
        }

        public SeperatedGaussianFilter(int size, double sigma)
        {
            _size = size;
            var templateTemp = new double[size];
            double sum = 0;

            //Func<int, int, int> calculateIndex = (x, y) => CalculateIndex(x, y, size, size);

            var centre = size / 2;

            for (var i = 0; i < size; i++)
            {
                templateTemp[i] = (Math.Exp(-((i - centre) * (i - centre)) / (2 * sigma * sigma)));
                sum += templateTemp[i];
            }

            _template = templateTemp.Select(t => t / sum).ToArray();
        }

        public override PgmImage Filter(PgmImage input)
        {
            var output = new PgmImage {Header = input.Header, Data = new byte[input.Data.Length]};

            var tempData = Convolve(_template, input.Data, new Size(1, _size), new Size(input.Header.Width, input.Header.Height));
            output.Data = Convolve(_template, tempData, new Size(_size, 1), new Size(input.Header.Width, input.Header.Height));

            return output;
        }

        public override string ToString()
        {
            return string.Format("Gauss {0}", _sigma);
        }
    }
}
