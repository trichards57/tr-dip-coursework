namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    using System;
    using System.Threading.Tasks;

    using Images;

    /// <summary>
    /// A filter object to apply the Laplacian Operator to an image.
    /// </summary>
    public static class LaplacianOperator
    {
        /// <summary>
        /// Applies the Laplacian Operator to the given image.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <returns>The processed image</returns>
        /// <remarks>
        /// Algorithm taken from @cite imageProcessingBook
        /// </remarks>
        public static ImageData Filter(ImageData input)
        {
            var output = new ImageData(input.Width, input.Height);

            // Produce the template for the Laplacian
            var template = new[] { 0, -1, 0, -1, 4, -1, 0, -1, 0 };

            // Convolve it across the image
            output.Data = ImageUtilities.Convolve(template, input.Data, new System.Drawing.Size(3, 3), new System.Drawing.Size(input.Width, input.Height));

            Parallel.For(0, output.Data.Length, i =>
                                                    {
                                                        output.Data[i] = Math.Abs(output.Data[i]);
                                                    });

            return output;
        }
    }
}
