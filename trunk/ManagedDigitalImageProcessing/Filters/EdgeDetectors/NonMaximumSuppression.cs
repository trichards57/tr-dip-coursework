using System;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    public sealed class NonMaximumSuppression : FilterBase
    {
        private struct CoordinatePair
        {
            public int X1;
            public int Y1;
            public int X2;
            public int Y2;
        }

        private CoordinatePair GetCoordinates(double angle)
        {
            var result = new CoordinatePair();
            var delta = 1e-14;

            result.X1 = (int)Math.Ceiling((Math.Cos(angle + Math.PI / 8) * Math.Sqrt(2)) - 0.5 - delta);
            result.Y1 = (int)Math.Ceiling((-Math.Sin(angle + Math.PI / 8) * Math.Sqrt(2)) - 0.5 - delta);
            result.X2 = (int)Math.Ceiling((Math.Cos(angle - Math.PI / 8) * Math.Sqrt(2)) - 0.5 - delta);
            result.Y2 = (int)Math.Ceiling((-Math.Sin(angle - Math.PI / 8) * Math.Sqrt(2)) - 0.5 - delta);

            return result;
        }

        public NonMaximumResult Filter(FilterResult input)
        {
            var result = new NonMaximumResult { XData = input.XData, YData = input.YData, Peak = new byte[input.XData.Length], Header = input.Header };

            Func<int, int, int> calculateIndex = (x, y) => CalculateIndex(x, y, input.Header.Width, input.Header.Height);

            Func<double, double, double> magnitude = (x, y) => Math.Sqrt(x * x + y * y);
            Func<int, int, double> pointMagnitude = (x, y) => magnitude(input.XData[calculateIndex(x, y)], input.YData[calculateIndex(x, y)]);
            Func<double, double, double> direction = (x, y) => Math.Atan(x / y);

            for (var i = 0; i < input.Header.Width; i++)
            {
                for (var j = 0; j < input.Header.Height; j++)
                {
                    byte mx = input.XData[calculateIndex(i, j)];
                    byte my = input.YData[calculateIndex(i, j)];

                    double angle;

                    if (my != 0)
                        angle = direction(mx, my);
                    else if (my == 0 && mx > 0)
                        angle = Math.PI / 2;
                    else
                        angle = -Math.PI / 2;

                    var coords = GetCoordinates(angle);
                    var m1 = (my) * pointMagnitude(i + coords.X1, j + coords.Y1) + ((mx - my)) * pointMagnitude(i + coords.X2, j + coords.Y2);
                    coords = GetCoordinates(angle + Math.PI);
                    var m2 = (my) * pointMagnitude(i + coords.X1, j + coords.Y1) + ((mx - my)) * pointMagnitude(i + coords.X2, j + coords.Y2);

                    var normalAngle = angle + Math.PI / 2;
                    if (normalAngle > 2 * Math.PI)
                        normalAngle -= 2 * Math.PI;
                    if (normalAngle < 0)
                        normalAngle += 2 * Math.PI;

                    //var m1 = (my / mx) * magnitude(input.XData[calculateIndex(i + 1, j - 1)], input.YData[calculateIndex(i + 1, j - 1)]) + ((mx - my) / mx) * magnitude(input.XData[calculateIndex(i, j - 1)], input.YData[calculateIndex(i, j - 1)]);
                    //var m2 = (my / mx) * magnitude(input.XData[calculateIndex(i - 1, j + 1)], input.YData[calculateIndex(i - 1, j + 1)]) + ((mx - my) / mx) * magnitude(input.XData[calculateIndex(i, j + 1)], input.YData[calculateIndex(i, j + 1)]);


                    Func<double, byte> limit = (x =>
                        {
                            if (x > 255)
                                return 255;
                            else if (x < 0)
                                return 0;
                            else
                                return (byte)x;
                        });

                    if ((mx * magnitude(mx,my) > m1 && mx * magnitude(mx,my)>= m2) || ((mx*magnitude(mx,my)<m1 && mx*magnitude(mx,my)<=m2)))
                    {
                        checked
                        {
                            result.Peak[calculateIndex(i, j)] = limit(magnitude(mx, my));
                        }
                    }
                    else
                        result.Peak[calculateIndex(i, j)] = 0;
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
