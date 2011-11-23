namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    using Images;

    /// <summary>
    /// A filter class to apply the open morphological operator to an image.
    /// </summary>
    public static class Open
    {
        /// <summary>
        /// Applies the open operator to a specified image.
        /// </summary>
        /// <param name="image">The input image.</param>
        /// <param name="erodeSize">Size of the erode structuring element.</param>
        /// <param name="dilateSize">Size of the dilate structuring element.</param>
        /// <returns>
        /// The opened image
        /// </returns>
        /// <remarks>
        /// Algorithm taken from @cite lectures
        /// </remarks>
        public static ImageData Filter(ImageData image, int erodeSize, int dilateSize)
        {
            return Dilate.Filter(Erode.Filter(image, erodeSize), dilateSize);
        }

        /// <summary>
        /// Applies the open operator to a specified image.
        /// </summary>
        /// <param name="image">The input image.</param>
        /// <param name="structuringElementSize">Size of the structuring elements.</param>
        /// <returns>The opened image</returns>
        public static ImageData Filter(ImageData image, int structuringElementSize)
        {
            return Filter(image, structuringElementSize, structuringElementSize);
        }
    }
}
