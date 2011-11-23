namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    using System;
    using System.Drawing;
    using System.Threading.Tasks;

    using Images;

    /// <summary>
    /// Filter class to apply a Gaussian filter to an image (using a seperated template)
    /// </summary>
    public static class SeperatedGaussianFilter
    {
        /// <summary>
        /// Applies the Gaussian filter to the supplied image.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <param name="sigma">The sigma value to use.</param>
        /// <returns>The filtered image</returns>
        public static ImageData Filter(ImageData input, double sigma)
        {
            return Filter(input, (int)((2 * Math.Ceiling(3 * sigma)) + 1), sigma);
        }

        /// <summary>
        /// Applies the Gaussian filter to the supplied image.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <param name="size">The size of the template to use.</param>
        /// <param name="sigma">The sigma value to use.</param>
        /// <returns>The filtered image</returns>
        /// <remarks>
        /// Algorithm taken from @cite lectures
        /// </remarks>
        public static ImageData Filter(ImageData input, int size, double sigma)
        {
            var templateTemp = new double[size];
            double sum = 0;

            var centre = size / 2;
            
            // Create the 1D template
            Parallel.For(
                0,
                size,
                i =>
                {
                    templateTemp[i] = Math.Exp(-((i - centre) * (i - centre)) / (2 * sigma * sigma));
                    sum += templateTemp[i];
                });

            // Normalise the template
            var template = new double[templateTemp.Length];
            Parallel.For(0, templateTemp.Length, i => template[i] = templateTemp[i] / sum);

            var output = new ImageData(input.Width, input.Height);

            // Convolve the template across the image, first in its vertical arrangement, then it's
            // horizontal arrangement.
            var tempData = ImageUtilities.Convolve(
                template, input.Data, new Size(1, size), new Size(input.Width, input.Height));
            output.Data = ImageUtilities.Convolve(
                template, tempData, new Size(size, 1), new Size(input.Width, input.Height));

            return output;
        }
    }
}
