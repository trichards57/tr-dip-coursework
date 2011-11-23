namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    using Images;

    /// <summary>
    /// Filter class, used to perform the canny edge detection on an image in one step.
    /// </summary>
    /// <remarks>
    /// This code skips the intial smoothing/filtering step, which should be performed on the data before it is passed in.
    /// </remarks>
    public static class CannyFilter
    {
        /// <summary>
        /// Applies the Canny Edge Detector to the input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="highThreshold">The high threshold.</param>
        /// <param name="lowThreshold">The low threshold.</param>
        /// <returns>
        /// The edge detected output.
        /// </returns>
        /// <remarks>
        /// Runs the following filters in order:
        /// 
        /// -# SobelOperator
        /// -# NonMaximumSuppression
        /// -# HysteresisThresholding
        /// 
        /// Algorithm taken from @cite imageProcessingBook
        /// </remarks>
        public static ImageData Filter(ImageData input, int highThreshold, int lowThreshold)
        {
            return HysteresisThresholding.Filter(NonMaximumSuppression.Filter(SobelOperator.FilterSplit(input)), highThreshold, lowThreshold);
        }
    }
}
