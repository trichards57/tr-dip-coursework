namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    using System.Linq;
    using Images;
    using NoiseReduction;

    /// <summary>
    /// Filter class to apply a morpholgical gradient operation to an image, highlighting the edges.
    /// </summary>
    public static class MorphologicalGradient
    {
        /// <summary>
        /// Filters the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="erodeElementSize">Size of the erode.</param>
        /// <param name="dilateElementSize">Size of the dilate.</param>
        /// <returns>
        /// An image with the edges highlighted by the morphological gradient operator.
        /// </returns>
        /// <remarks>
        /// Algorithm taken from @cite imageProcessingBook
        /// </remarks>
        public static ImageData Filter(ImageData image, int erodeElementSize, int dilateElementSize)
        {
            // First create two images, one an erosion of the original and one a dilation
            var er = Erode.Filter(image, erodeElementSize);
            var di = Dilate.Filter(image, dilateElementSize);

            // Now take the differences of the two images to create a new image
            var buffer =
                di.Data.AsParallel().AsOrdered().Zip(er.Data.AsParallel().AsOrdered(), (d, e) => d - e).ToArray();

            return new ImageData { Width = image.Width, Height = image.Height, Data = buffer };
        }

        /// <summary>
        /// Filters the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="structuringElementSize">Size of the structuring element.</param>
        /// <returns>An image with the edges highlighted by the morphological gradient operator.</returns>
        public static ImageData Filter(ImageData image, int structuringElementSize)
        {
            return Filter(image, structuringElementSize, structuringElementSize);
        }
    }
}
