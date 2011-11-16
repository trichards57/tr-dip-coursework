// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NonMaximumSuppression.cs" company="Tony Richards">
//   Copyright (c) 2011, Tony Richards
//   All rights reserved.
//
//   Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
//   following conditions are met:
//
//   Redistributions of source code must retain the above copyright notice, this list of conditions and the following 
//   disclaimer.
//   
//   Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the 
//   following disclaimer in the documentation and/or other materials provided with the distribution.
//   
//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
//   INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
//   DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
//   SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
//   SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//   WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE 
//   USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// <summary>
//   Defines the NonMaximumSuppression type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Filter class to remove any non-maximum edges in the input data.
    /// </summary>
    public sealed class NonMaximumSuppression : FilterBase
    {
        /// <summary>
        /// Applies non-maximum suppression to the specified input.
        /// </summary>
        /// <param name="input">The input data.</param>
        /// <returns>The non maximum suppressed result.</returns>
        /// <remarks>
        /// Algorithm taken from @cite imageProcessingBook
        /// </remarks>
        public NonMaximumResult Filter(SobelOperatorResult input)
        {
            var result = new NonMaximumResult { XData = input.XData, YData = input.YData, Peak = new byte[input.XData.Length], Width = input.Width, Height = input.Height };

            // Convenience function to calculate the index of an entry
            Func<int, int, int> calculateIndex = (x, y) => CalculateIndex(x, y, input.Width, input.Height);
            
            // Convenience function to calculate the magnitude of a vector
            Func<double, double, double> magnitude = (x, y) => Math.Sqrt((x * x) + (y * y));

            // Convenience function to transform magnitude(x,y) to accept a coordinate as an input
            Func<int, int, double> pointMagnitude = (x, y) => magnitude(input.XData[calculateIndex(x, y)], input.YData[calculateIndex(x, y)]);

            // Convenience function to get the direction from a vector
            Func<double, double, double> direction = (x, y) => Math.Atan(x / y);

            Parallel.For(
                0, 
                input.Width, 
                i =>
            {
                for (var j = 0; j < input.Height; j++)
                {
                    // Moving through every row and column
                    // Keep the data in local variables to clean up the code
                    var mx = input.XData[calculateIndex(i, j)];
                    var my = input.YData[calculateIndex(i, j)];

                    double angle;

                    // Calculate the edge direction
                    if (my != 0)
                    {
                        angle = direction(mx, my);
                    }

                    // Allowing for the my == 0 and mx == 0 cases
                    else if (my == 0 && mx > 0)
                    {
                        angle = Math.PI / 2;
                    }
                    else
                    {
                        angle = -Math.PI / 2;
                    }

                    // Work out the two points normal to the edge
                    var coords = GetCoordinates(angle);
                    var m1 = (my * pointMagnitude(i + coords.X1, j + coords.Y1)) + ((mx - my) * pointMagnitude(i + coords.X2, j + coords.Y2));
                    coords = GetCoordinates(angle + Math.PI);
                    var m2 = (my * pointMagnitude(i + coords.X1, j + coords.Y1)) + ((mx - my) * pointMagnitude(i + coords.X2, j + coords.Y2));

                    // Convenience function to truncate a number to between 0 and 255 to fit in one byte
                    Func<double, byte> limit = x =>
                                                    {
                                                        if (x > 255)
                                                        {
                                                            return 255;
                                                        }

                                                        if (x < 0)
                                                        {
                                                            return 0;
                                                        }

                                                        return (byte)x;
                                                    };

                    // If the point is at the maximum of it's edge, set the pixel to it's value
                    if ((mx * magnitude(mx, my) > m1 && mx * magnitude(mx, my) >= m2) || (mx * magnitude(mx, my) < m1 && mx * magnitude(mx, my) <= m2))
                    {
                        checked
                        {
                            result.Peak[calculateIndex(i, j)] = limit(magnitude(mx, my));
                        }
                    }

                    // Otherwise set the value to 0, suppressing that edge point.
                    else
                    {
                        result.Peak[calculateIndex(i, j)] = 0;
                    }
                }
            });

            return result;
        }

        /// <summary>
        /// Gets the coordinates of two points normal to a line on the given angle.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>A pair of coordinates</returns>
        /// <remarks>
        /// Algorithm taken from @cite imageProcessingBook
        /// </remarks>
        private static CoordinatePair GetCoordinates(double angle)
        {
            var result = new CoordinatePair();
            const double Delta = 1e-14;

            result.X1 = (int)Math.Ceiling((Math.Cos(angle + (Math.PI / 8)) * Math.Sqrt(2)) - 0.5 - Delta);
            result.Y1 = (int)Math.Ceiling((-Math.Sin(angle + (Math.PI / 8)) * Math.Sqrt(2)) - 0.5 - Delta);
            result.X2 = (int)Math.Ceiling((Math.Cos(angle - (Math.PI / 8)) * Math.Sqrt(2)) - 0.5 - Delta);
            result.Y2 = (int)Math.Ceiling((-Math.Sin(angle - (Math.PI / 8)) * Math.Sqrt(2)) - 0.5 - Delta);

            return result;
        }

        /// <summary>
        /// Structure to hold a pair of coordinates.
        /// </summary>
        private struct CoordinatePair
        {
            /// <summary>
            /// The first X coordinate
            /// </summary>
            public int X1;

            /// <summary>
            /// The first Y coordinate
            /// </summary>
            public int Y1;

            /// <summary>
            /// The second X coordinate
            /// </summary>
            public int X2;

            /// <summary>
            /// The second Y coordinate
            /// </summary>
            public int Y2;
        }
    }
}
