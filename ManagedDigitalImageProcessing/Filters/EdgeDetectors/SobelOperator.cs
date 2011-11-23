namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    using System;
    using System.Drawing;
    using System.Linq;

    using Images;

    /// <summary>
    /// Filter class, used to run the Sobel operator on an image.
    /// </summary>
    public static class SobelOperator
    {
        /// <summary>
        /// Applies the Sobel operator to the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The filtered image.</returns>
        public static ImageData Filter(ImageData input)
        {
            // Calculate the edge vectors.
            var outputTemp = FilterSplit(input);

            // Transform, in parallel, the calculated vectors in to their magnitudes, and create the output data
            var output = new ImageData
                {
                    Height = input.Height,
                    Width = input.Width,
                    Data =
                        outputTemp.XData.AsParallel().AsOrdered().Zip(
                            outputTemp.YData.AsParallel().AsOrdered(),
                            (x, y) => (int)Math.Round(Math.Sqrt((x * x) + (y * y)))).ToArray()
                };
            
            return output;
        }

        /// <summary>
        /// Applies the Sobel operator to the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The edge vectors from the filtered image.</returns>
        public static SobelOperatorResult FilterSplit(ImageData input)
        {
            var output = new SobelOperatorResult { Width = input.Width, Height = input.Height };

            // Define the templates.
            var template1 = new[] { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
            var template2 = new[] { 1, 2, 1, 0, 0, 0, -1, -2, -1 };

            var intData = input.Data;

            // Convolve the first template across the image, using the absolute value to ensure edges in both direction are treated equally
            var data = ImageUtilities.Convolve(template1, intData, new Size(3, 3), new Size(input.Width, input.Height));
            output.XData = data.AsParallel().Select(Math.Abs).ToArray();

            // Do the same again for the second template.
            data = ImageUtilities.Convolve(template2, intData, new Size(3, 3), new Size(input.Width, input.Height));
            output.YData = data.AsParallel().Select(Math.Abs).ToArray();

            return output;
        }
    }
}
