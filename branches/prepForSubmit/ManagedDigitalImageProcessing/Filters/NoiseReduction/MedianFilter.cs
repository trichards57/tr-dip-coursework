namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    using System;
    using System.Threading.Tasks;

    using Images;

    /// <summary>
    /// Filter class to apply a median filter to the input.
    /// </summary>
    public static class MedianFilter
    {
        /// <summary>
        /// Filters the specified input.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <param name="windowSize">Size of the filter window.</param>
        /// <returns>
        /// The filtered image.
        /// </returns>
        /// <remarks>
        /// Algorithm taken from @cite lectures
        /// </remarks>
        public static ImageData Filter(ImageData input, int windowSize = 3)
        {
            if (windowSize % 2 == 0)
            {
                throw new ArgumentOutOfRangeException("windowSize", windowSize, "The size must be odd.");
            }

            var output = new ImageData(input.Width, input.Height);

            // Partial function application to simplify index calculation.
            Func<int, int, int> calculateOutputIndex = (x, y) => ImageUtilities.CalculateIndex(x, y, output.Width);
            Func<int, int, int> calculateIndex = (x, y) => ImageUtilities.CalculateIndex(x, y, input.Width, input.Height);

            var offset = windowSize / 2;

            // Iterate through each column.
            Parallel.For(
                0,
                output.Width,
                i =>
                {
                    // Iterate through each point in the column.
                    for (var j = 0; j < output.Height; j++)
                    {
                        var list = new int[windowSize * windowSize];
                        var count = 0;

                        // Iterate through each of the items in the window, adding the value to a list.
                        for (var k = -offset; k <= offset; k++)
                        {
                            for (var l = -offset; l <= offset; l++)
                            {
                                list[count++] = input.Data[calculateIndex(i + k, j + l)];
                            }
                        }

                        // Sort the list.
                        Array.Sort(list);

                        // Take the middle value (the median) and insert it in to the new image.
                        output.Data[calculateOutputIndex(i, j)] = list[list.Length / 2];
                    }
                });

            return output;
        }
    }
}
