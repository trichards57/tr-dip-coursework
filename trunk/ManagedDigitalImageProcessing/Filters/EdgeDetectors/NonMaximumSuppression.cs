using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    class NonMaximumSuppression : FilterBase
    {
        public NonMaximumResult Filter(FilterResult input)
        {
            var result = new NonMaximumResult { XData = input.XData, YData = input.YData, Peak = new bool[input.XData.Length], Header = input.Header};

            Func<int, int, int> calculateIndex = (x, y) => CalculateIndex(x, y, input.Header.Width, input.Header.Height);

            Func<double, double, double> magnitude = (x, y) => Math.Sqrt(x * x + y * y);

            for (int i = 0; i < input.Header.Width; i++)
            {
                for (int j = 0; j < input.Header.Height; j++)
                {
                    double mx = input.XData[calculateIndex(i,j)];
                    double my = input.YData[calculateIndex(i,j)];
                    var m1 = (my / mx) * magnitude(input.XData[calculateIndex(i + 1, j - 1)], input.YData[calculateIndex(i + 1, j - 1)]) + ((mx - my) / mx) * magnitude(input.XData[calculateIndex(i, j - 1)], input.YData[calculateIndex(i, j - 1)]);
                    var m2 = (my / mx) * magnitude(input.XData[calculateIndex(i - 1, j + 1)], input.YData[calculateIndex(i - 1, j + 1)]) + ((mx - my) / mx) * magnitude(input.XData[calculateIndex(i, j + 1)], input.YData[calculateIndex(i, j + 1)]);

                    if (magnitude(mx, my) > m1 && magnitude(mx, my) > m2)
                        result.Peak[calculateIndex(i, j)] = true;
                    else
                        result.Peak[calculateIndex(i, j)] = false;
                }
            }

            return result;
        }

        public override PgmImage Filter(PgmImage input)
        {
            throw new NotImplementedException();
        }
    }
}
