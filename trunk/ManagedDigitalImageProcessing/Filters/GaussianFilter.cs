using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.PGM;
using System.Drawing;

namespace ManagedDigitalImageProcessing.Filters
{
    class GaussianFilter : FilterBase
    {
        private double[] template;
        private int size;
        private double sigma;

        public GaussianFilter(double sigma) : this((int)(2 * Math.Ceiling(3 * sigma) + 1), sigma)
        {
            this.sigma = sigma;
        }

        public GaussianFilter(int size, double sigma)
        {
            this.size = size;
            var templateTemp = new double[size * size];
            double sum = 0;

            Func<int, int, int> calculateIndex = (x, y) => CalculateIndex(x, y, size, size);

            var centre = size / 2;

            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    templateTemp[calculateIndex(i, j)] = Math.Exp(-(((j - centre) * (j - centre)) + ((i - centre) * (i - centre))) / (2 * sigma * sigma));
                    sum += templateTemp[calculateIndex(i, j)];
                }
            }

            template = templateTemp.Select(t => t / sum).ToArray();
        }

        public override PGM.PgmImage Filter(PGM.PgmImage input)
        {
            var output = new PgmImage();
            output.Header = input.Header;
            output.Data = new byte[input.Data.Length];

            output.Data = Convolve(template, input.Data, new Size(size, size), new Size(input.Header.Width, input.Header.Height));

            return output;
        }

        public override string ToString()
        {
            return string.Format("Gauss {0}", sigma);
        }
    }
}
