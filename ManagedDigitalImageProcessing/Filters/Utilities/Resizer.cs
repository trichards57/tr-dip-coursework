using System;
using ManagedDigitalImageProcessing.PGM;

// From http://paulbourke.net/texture_colour/imageprocess/

namespace ManagedDigitalImageProcessing.Filters.Utilities
{
    class Resizer : FilterBase
    {
        private readonly int _targetWidth;
        private readonly int _targetHeight;

        public Resizer(int targetWidth, int targetHeight)
        {
            _targetHeight = targetHeight;
            _targetWidth = targetWidth;
        }

        public virtual PgmImage Filter(PgmImage input)
        {
            var width = input.Header.Width;
            var height = input.Header.Height;
            Func<int, int, int> originalIndex = ((x, y) => CalculateIndex(x, y, width, height));
            Func<int, int, int> newIndex = ((x, y) => CalculateIndex(x, y, _targetWidth, _targetHeight));

            if (_targetWidth == width && _targetHeight == height)
                return input;

            var output = new byte[_targetHeight * _targetWidth];

            for (var i = 0; i < _targetWidth; i++)
            {
                for (var j = 0; j < _targetHeight; j++)
                {
                    var x = i * ((double)width / _targetWidth);
                    var y = j * ((double)height / _targetHeight);

                    var iDash = (int)Math.Floor(x);
                    var jDash = (int)Math.Floor(y);

                    var dx = x - iDash;
                    var dy = y - jDash;

                    double sum = 0;

                    for (var m = -1; m <= 2; m++)
                    {
                        for (var n = -1; n <= 2; n++)
                        {
                            sum += (input.Data[originalIndex(iDash + m, jDash + n)] * Weighting(m - dx) * Weighting(dy - n));
                        }
                    }

                    checked
                    {
                        output[newIndex(i, j)] = (byte)sum;
                    }
                }
            }

            return new PgmImage { Data = output, Header = new PgmHeader { Height = _targetHeight, Width = _targetWidth } };
        }

        private static double Weighting(double x)
        {
            Func<double, double> p = (i => i > 0 ? i : 0);

            return (1.0 / 6) *
                   (Math.Pow(p(x + 2), 3) - 4 * Math.Pow(p(x + 1), 3) + 6 * Math.Pow(p(x), 3) - 4 * Math.Pow(p(x - 1), 3));
        }
    }
}
