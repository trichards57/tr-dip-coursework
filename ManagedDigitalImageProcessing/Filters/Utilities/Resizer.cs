namespace ManagedDigitalImageProcessing.Filters.Utilities
{
    using System;
    using System.Threading.Tasks;

    using Images;

    /// <summary>
    /// A filter class that resizes the image using bicubic interpolation
    /// </summary>
    public static class Resizer
    {
        /// <summary>
        /// Resizes the specified input.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <param name="targetWidth">Target width of the image.</param>
        /// <param name="targetHeight">Target height of the image.</param>
        /// <returns>
        /// The resized image.
        /// </returns>
        /// <remarks>
        /// Algorithm taken from @cite resizeAlgorithm
        /// </remarks>
        public static ImageData Filter(ImageData input, int targetWidth, int targetHeight)
        {
            var width = input.Width;
            var height = input.Height;

            // Convenience functions to ease the calculation of array indexes
            Func<int, int, int> originalIndex = (x, y) => ImageUtilities.CalculateIndex(x, y, width, height);
            Func<int, int, int> newIndex = (x, y) => ImageUtilities.CalculateIndex(x, y, targetWidth);

            var output = new int[targetHeight * targetWidth];

            Parallel.For(
                0,
                targetWidth,
                i =>
                    {
                        for (var j = 0; j < targetHeight; j++)
                        {
                            // For each pixel in the output image, calculate the original pixel it is derived from
                            var x = i * ((double)width / targetWidth);
                            var y = j * ((double)height / targetHeight);

                            // Convert these values to actual pixel values (which must be integers).
                            var newI = (int)Math.Floor(x);
                            var newJ = (int)Math.Floor(y);

                            // Work out the difference between the actual pixel coordinates and the calculated ones
                            var dx = x - newI;
                            var dy = y - newJ;

                            double sum = 0;

                            // Look at the surrounding pixels and, using the weights calculate the new pixel value
                            for (var m = -1; m <= 2; m++)
                            {
                                for (var n = -1; n <= 2; n++)
                                {
                                    sum += input.Data[originalIndex(newI + m, newJ + n)] * Weighting(m - dx)
                                           * Weighting(dy - n);
                                }
                            }

                            // Convert the sum back to an integer to fit in the image.
                            checked
                            {
                                output[newIndex(i, j)] = (int)sum;
                            }
                        }
                    });

            return new ImageData { Data = output, Width = targetWidth, Height = targetHeight };
        }

        /// <summary>
        /// The cubic weighting function
        /// </summary>
        /// <param name="x">The input value.</param>
        /// <returns>The weight for the interpolation.</returns>
        private static double Weighting(double x)
        {
            Func<double, double> p = i => i > 0 ? i : 0;

            return (Math.Pow(p(x + 2), 3) - (4 * Math.Pow(p(x + 1), 3)) + (6 * Math.Pow(p(x), 3)) - (4 * Math.Pow(p(x - 1), 3))) / 6.0;
        }
    }
}
