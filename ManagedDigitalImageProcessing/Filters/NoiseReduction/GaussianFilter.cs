namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    using System;
    using System.Drawing;
    using System.Threading.Tasks;

    using Images;

    /// <summary>
    /// Filter class to apply a Gaussian smoothing filter to an image.
    /// </summary>
    public static class GaussianFilter
    {
        /// <summary>
        /// Applies the Gaussian filter to the image.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <param name="sigma">The sigma value for the template.</param>
        /// <returns>The filtered image</returns>
        /// <remarks>
        /// <para>Sizes the template to ensure that \f$3\sigma\f$ can fit inside it.</para>
        /// <para>Algorithm taken from @cite lectures</para> 
        /// </remarks>
        public static ImageData Filter(ImageData input, double sigma)
        {
            return Filter(input, (int)((2 * Math.Ceiling(3 * sigma)) + 1), sigma);
        }

        /// <summary>
        /// Applies the Guassian filter to the image.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <param name="size">The size of the template.</param>
        /// <param name="sigma">The sigma value for the template.</param>
        /// <returns>
        /// The filtered image.
        /// </returns>
        /// <remarks>
        /// Template algorithm taken from @cite imageProcessingBook
        /// </remarks>
        public static ImageData Filter(ImageData input, int size, double sigma)
        {
            var templateTemp = new double[size * size];
            double sum = 0;

            Func<int, int, int> calculateIndex = (x, y) => ImageUtilities.CalculateIndex(x, y, size);

            var centre = size / 2;

            // Produce the template
            Parallel.For(
                0,
                size,
                i =>
                {
                    for (var j = 0; j < size; j++)
                    {
                        templateTemp[calculateIndex(i, j)] =
                            Math.Exp(-(((j - centre) * (j - centre)) + ((i - centre) * (i - centre))) /
                                     (2 * sigma * sigma));
                        sum += templateTemp[calculateIndex(i, j)];
                    }
                });

            // Normalise the template
            var template = new double[templateTemp.Length];
            Parallel.For(0, templateTemp.Length, i => template[i] = templateTemp[i] / sum);

            // Apply the template to the image.
            var output = new ImageData
                {
                    Width = input.Width,
                    Height = input.Height,
                    Data =
                        ImageUtilities.Convolve(
                            template, input.Data, new Size(size, size), new Size(input.Width, input.Height))
                };

            return output;
        }
    }
}
