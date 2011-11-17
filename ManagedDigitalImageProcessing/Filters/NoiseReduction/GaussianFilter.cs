// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GaussianFilter.cs" company="Tony Richards">
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
//   Defines the GaussianFilter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    using System;
    using System.Drawing;
    using System.Threading.Tasks;

    using ManagedDigitalImageProcessing.Images;

    /// <summary>
    /// Filter class to apply a Gaussian smoothing filter to an image.
    /// </summary>
    public sealed class GaussianFilter
    {
        /// <summary>
        /// The filter template
        /// </summary>
        private readonly double[] template;

        /// <summary>
        /// The size of the filter template
        /// </summary>
        private readonly int size;

        /// <summary>
        /// The sigma value used by the gaussian function
        /// </summary>
        private readonly double sigma;

        /// <summary>
        /// Initializes a new instance of the <see cref="GaussianFilter"/> class.
        /// </summary>
        /// <param name="sigma">The sigma value to use.</param>
        /// <remarks>
        /// This constructor sizes the filter to ensure it can fit three \f$\sigma\f$ and will be odd sized.
        /// </remarks>
        public GaussianFilter(double sigma)
            : this((int)((2 * Math.Ceiling(3 * sigma)) + 1), sigma)
        {
            this.sigma = sigma;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GaussianFilter"/> class.
        /// </summary>
        /// <param name="size">The size of the value.</param>
        /// <param name="sigma">The sigma value to use.</param>
        /// <remarks>
        /// Template generation code taken from @cite imageProcessingBook
        /// </remarks>
        public GaussianFilter(int size, double sigma)
        {
            this.size = size;
            var templateTemp = new double[size * size];
            double sum = 0;

            Func<int, int, int> calculateIndex = (x, y) => ImageUtilities.CalculateIndex(x, y, size);

            var centre = size / 2;

            Parallel.For(
                0,
                size,
                i =>
                {
                    for (var j = 0; j < size; j++)
                    {
                        templateTemp[calculateIndex(i, j)] =
                            Math.Exp(-(((j - centre) * (j - centre)) + ((i - centre) * (i - centre))) /
                                     (2 * sigma * sigma));
                        sum += templateTemp[calculateIndex(i, j)];
                    }
                });

            template = new double[templateTemp.Length];
            Parallel.For(0, templateTemp.Length, i => template[i] = templateTemp[i] / sum);
        }

        /// <summary>
        /// Applies the Guassian filter to the image.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <returns>The filtered image.</returns>
        public ImageData Filter(ImageData input)
        {
            var output = new ImageData
                {
                    Width = input.Width,
                    Height = input.Height,
                    Data =
                        ImageUtilities.Convolve(
                            this.template, input.Data, new Size(this.size, this.size), new Size(input.Width, input.Height))
                };


            return output;
        }
    }
}
