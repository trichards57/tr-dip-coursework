namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    using Images;

    /// <summary>
    /// A filter class to apply the close morphological operator to an image.
    /// </summary>
    public static class Close
    {
        /// <summary>
        /// Applies the close operator to the input image.
        /// </summary>
        /// <param name="image">The input image.</param>
        /// <param name="erodeSize">Size of the erode structuring element.</param>
        /// <param name="dilateSize">Size of the dilate structuring element.</param>
        /// <returns>
        /// The closed image
        /// </returns>
        /// <remarks>
        /// Algorithm taken from @cite lectures
        /// </remarks>
        public static ImageData Filter(ImageData image, int erodeSize, int dilateSize)
        {
            // Closing is a dilation followed by an erosion
            return Erode.Filter(Dilate.Filter(image, dilateSize), erodeSize);
        }

        /// <summary>
        /// Filters the specified image, using equal sized structuring elements.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="structuringElementSize">Size of the structuring elements.</param>
        /// <returns>
        /// The closed image
        /// </returns>
        public static ImageData Filter(ImageData image, int structuringElementSize)
        {
            return Filter(image, structuringElementSize, structuringElementSize);
        }
    }
}
