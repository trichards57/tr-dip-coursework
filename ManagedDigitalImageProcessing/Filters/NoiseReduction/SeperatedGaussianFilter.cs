// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeperatedGaussianFilter.cs" company="Tony Richards">
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
//   Defines the SeperatedGaussianFilter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    using System;
    using System.Drawing;
    using System.Threading.Tasks;

    using ManagedDigitalImageProcessing.Images;

    /// <summary>
    /// Filter class to apply a Gaussian filter to an image (using a seperated template)
    /// </summary>
    public static class SeperatedGaussianFilter
    {
        /// <summary>
        /// Applies the Gaussian filter to the supplied image.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <param name="sigma">The sigma value to use.</param>
        /// <returns>The filtered image</returns>
        public static ImageData Filter(ImageData input, double sigma)
        {
            return Filter(input, (int)((2 * Math.Ceiling(3 * sigma)) + 1), sigma);
        }

        /// <summary>
        /// Applies the Gaussian filter to the supplied image.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <param name="size">The size of the template to use.</param>
        /// <param name="sigma">The sigma value to use.</param>
        /// <returns>The filtered image</returns>
        public static ImageData Filter(ImageData input, int size, double sigma)
        {
            var templateTemp = new double[size];
            double sum = 0;

            var centre = size / 2;

            Parallel.For(
                0,
                size,
                i =>
                {
                    templateTemp[i] = Math.Exp(-((i - centre) * (i - centre)) / (2 * sigma * sigma));
                    sum += templateTemp[i];
                });

            var template = new double[templateTemp.Length];
            Parallel.For(0, templateTemp.Length, i => template[i] = templateTemp[i] / sum);

            var output = new ImageData(input.Width, input.Height);

            var tempData = ImageUtilities.Convolve(
                template, input.Data, new Size(1, size), new Size(input.Width, input.Height));
            output.Data = ImageUtilities.Convolve(
                template, tempData, new Size(size, 1), new Size(input.Width, input.Height));

            return output;
        }
    }
}
