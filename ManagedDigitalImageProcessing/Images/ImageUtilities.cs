﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageUtilities.cs" company="Tony Richards">
//   Copyright (c) 2011, Tony Richards
//   All rights reserved.
//
//   Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
//   following conditions are met:
//
//   Redistributions of source code must retain the above copyright notice, this list of conditions and the following 
//   disclaimer.
//   
//   Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the 
//   following disclaimer in the documentation and/or other materials provided with the distribution.
//   
//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
//   INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
//   DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
//   SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
//   SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//   WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE 
//   USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// <summary>
//   A base class to allow the CalculateIndex function to be reused.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.Images
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// A base class to allow the CalculateIndex function to be reused.
    /// </summary>
    public static class ImageUtilities
    {
        /// <summary>
        /// Calculates the index of a point at given coordinates.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <returns>An index to access the given point in a 1D array.</returns>
        /// <remarks>
        /// Points that fall out of the range of the image are reflected back in to the image (only works for
        /// positions less than width or height out of range).
        /// </remarks>
        public static int CalculateIndex(int x, int y, int width, int height)
        {
            if (x < 0)
            {
                x = -x;
            }

            if (x >= width)
            {
                x = width - (x - width) - 1;
            }

            if (y < 0)
            {
                y = -y;
            }

            if (y >= height)
            {
                y = height - (y - height) - 1;
            }

            return x + (y * width);
        }

        /// <summary>
        /// Calculates the index of a point at given coordinates.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <returns>An index to access the given point in a 1D array.</returns>
        public static int CalculateIndex(int x, int y, int width)
        {
            return x + (y * width);
        }

        /// <summary>
        /// Convolves the specified input array with the specified data.
        /// </summary>
        /// <param name="input">The input array.</param>
        /// <param name="data">The data.</param>
        /// <param name="inputSize">Size of the input array.</param>
        /// <param name="dataSize">Size of the data array.</param>
        /// <returns>The result of the convolution</returns>
        public static int[] Convolve(IEnumerable<int> input, int[] data, Size inputSize, Size dataSize)
        {
            var doubleInput = input.Select(i => (double)i).ToArray();
            return Convolve(doubleInput, data, inputSize, dataSize);
        }

        /// <summary>
        /// Convolves the specified input array with the specified data.
        /// </summary>
        /// <param name="input">The input array.</param>
        /// <param name="data">The data.</param>
        /// <param name="inputSize">Size of the input array.</param>
        /// <param name="dataSize">Size of the data array.</param>
        /// <returns>The result of the convolution</returns>
        public static int[] Convolve(double[] input, int[] data, Size inputSize, Size dataSize)
        {
            Func<int, int, int> calculateInputIndex = (x, y) => CalculateIndex(x, y, inputSize.Width, inputSize.Height);
            Func<int, int, int> calculateDataIndex = (x, y) => CalculateIndex(x, y, dataSize.Width, dataSize.Height);
            Func<int, int, int> calculateOutputIndex = (x, y) => CalculateIndex(x, y, dataSize.Width);

            var output = new int[data.Length];
            var offsetX = inputSize.Width / 2;
            var offsetY = inputSize.Height / 2;

            Parallel.For(
                0,
                dataSize.Width,
                i =>
                    {
                        for (var j = 0; j < dataSize.Height; j++)
                        {
                            var sum = 0.0;
                            for (var k = 0; k < inputSize.Width; k++)
                            {
                                for (var l = 0; l < inputSize.Height; l++)
                                {
                                    sum += data[calculateDataIndex(i + k - offsetX, j + l - offsetY)]
                                           * input[calculateInputIndex(k, l)];
                                }
                            }

                            output[calculateOutputIndex(i, j)] = (int)Math.Floor(sum);
                        }
                    });
            return output;
        }
    }
}
