﻿namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Images;

    /// <summary>
    /// A filter object that applies Hysteresis Thresholding to the edges in an edge detected image, to join linked edges.
    /// </summary>
    /// <remarks>
    /// This makes up part of the CannyFilter.
    /// </remarks>
    /// <seealso cref="CannyFilter"/>
    public static class HysteresisThresholding
    {
        /// <summary>
        /// Applies hysteresis thresholding to the input image.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <param name="highThreshold">The high hysteresis threshold.</param>
        /// <param name="lowThreshold">The low hysteresis threshold.</param>
        /// <returns>
        /// The thresholded image.
        /// </returns>
        public static ImageData Filter(ImageData input, int highThreshold, int lowThreshold)
        {
            var output = new bool[input.Height * input.Width];

            var outImage = new ImageData(input.Width, input.Height);

            Func<int, int, int> calculateIndex = (x, y) => ImageUtilities.CalculateIndex(x, y, input.Width, input.Height);

            // Run through the image, searching for unprocessing pixels that are higher than the upper threshold, and so the
            // start of an edge
            Parallel.For(
                0,
                input.Width,
                i =>
                    {
                        for (var j = 0; j < input.Height; j++)
                        {
                            // If the pixel is below the higher threshold or has already been processed, skip it.
                            if (input.Data[calculateIndex(i, j)] <= highThreshold || output[calculateIndex(i, j)])
                            {
                                continue;
                            }

                            // Set this pixel to be an edge
                            output[calculateIndex(i, j)] = true;
                            
                            // Mark any connected pixels as edges if they are above the lower threshold.
                            output = Connect(i, j, output, input.Data, lowThreshold, calculateIndex);
                        }
                    });

            // Convert the boolean output back in to a displayable image.
            Parallel.For(0, output.Length, i => { outImage.Data[i] = output[i] ? 255 : 0; });

            return outImage;
        }

        /// <summary>
        /// Recursivly connects the point at the specified coordinate with any adjacent edges.
        /// </summary>
        /// <param name="x">The x coordinate of the pixel.</param>
        /// <param name="y">The y coordinate of the pixel.</param>
        /// <param name="output">The output state for each pixel.</param>
        /// <param name="input">The input value of each pixel.</param>
        /// <param name="lowThreshold">The low threshold for the hysteresis.</param>
        /// <param name="calculateIndex">The function to calculate the index of the pixel.</param>
        /// <returns>A list of boolean valeus that indicates if each pixel is part of a connected edge.</returns>
        /// <remarks>
        /// Algorithm taken from @cite imageProcessingBook
        /// </remarks>
        private static bool[] Connect(int x, int y, bool[] output, IList<int> input, int lowThreshold, Func<int, int, int> calculateIndex)
        {
            // Iterate through each pixel neighbouring the input pixel
            for (var i = x - 1; i < x + 2; i++)
            {
                for (var j = y - 1; j < y + 2; j++)
                {
                    // If the neighbouring pixel is below the threshold, or has already been processed and considered to be an edge, skip it
                    if (input[calculateIndex(i, j)] <= lowThreshold || output[calculateIndex(i, j)])
                    {
                        continue;
                    }

                    // Set the neighbouring pixel to be an edge
                    output[calculateIndex(i, j)] = true;
                    
                    // Process this new pixels neighbours
                    output = Connect(i, j, output, input, lowThreshold, calculateIndex);
                }
            }

            return output;
        }
    }
}
