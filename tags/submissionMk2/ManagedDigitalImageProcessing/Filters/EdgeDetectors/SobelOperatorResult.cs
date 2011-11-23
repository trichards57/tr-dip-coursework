namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    /// <summary>
    /// A class to hold the vector data returned by the Sobel operator.
    /// </summary>
    public sealed class SobelOperatorResult
    {
        /// <summary>
        /// Gets or sets the width of the image.
        /// </summary>
        /// <value>
        /// The width of the image.
        /// </value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the image.
        /// </summary>
        /// <value>
        /// The height of the image.
        /// </value>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the X  data.
        /// </summary>
        /// <value>
        /// The X data.
        /// </value>
        public int[] XData { get; set; }
        
        /// <summary>
        /// Gets or sets the Y data.
        /// </summary>
        /// <value>
        /// The Y data.
        /// </value>
        public int[] YData { get; set; }
    }
}
