// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FFTBandStop.cs" company="Tony Richards">
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
//   Defines the FFTBandStop type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FFT;
    using Utilities;
    using Images;

    /// <summary>
    /// Filter class to apply a bandstop filter using FFT techinques.
    /// </summary>
    public static class FFTBandStop
    {
        /// <summary>
        /// Applies the band stop to the specified image.
        /// </summary>
        /// <param name="input">The input image.</param>
        /// <param name="inner">The inner edge of the bandstop (in pixels).</param>
        /// <param name="outer">The outer edge of the bandstop (in pixels).</param>
        /// <returns>
        /// The band-stop filtered image
        /// </returns>
        public static ImageData Filter(ImageData input, int inner, int outer)
        {
            var width = input.Width;
            var height = input.Height;

            // Resize the image, ensuring that it has side dimensions 2^n
            // This does not preserve the image aspect ratio
            var newWidth = (int)Math.Pow(2, Math.Ceiling(Math.Log(width, 2)));
            var newHeight = (int)Math.Pow(2, Math.Ceiling(Math.Log(height, 2)));

            var resizedImage = Resizer.Filter(input, newWidth, newHeight);

            // Convert the image data to complex numbers (doesn't change the values)
            var complexInputData = new ComplexNumber[newWidth * newHeight];
            Parallel.For(0, newWidth * newHeight, i => complexInputData[i] = resizedImage.Data[i]);

            // Transform the image in to the frequency domain.
            var fftOutput = FFT.DitFFT2D(complexInputData, newWidth, newHeight);

            // Pre-square the inner and outer distances to save on calculating a lot of square roots later.
            var innerSquared = inner * inner;
            var outerSquared = outer * outer;

            // Work out the center of the image
            var centerX = (newWidth / 2.0) + 0.5;
            var centerY = (newHeight / 2.0) + 0.5;

            Parallel.For(
                0,
                newWidth,
                i =>
                {
                    // For every square in the image, if it's within the given anulus, set it to 0
                    for (var j = 0; j < newHeight; j++)
                    {
                        var distX = i - centerX;
                        var distY = j - centerY;
                        var distSquared = (distX * distX) + (distY * distY);
                        // Because the two distances have been squared, no need to square root here
                        if (distSquared > innerSquared && distSquared < outerSquared)
                        {
                            fftOutput[ImageUtilities.CalculateIndex(i, j, newWidth)] = 0;
                        }
                    }
                });
            // Perform the inverse transform on the image
            var ifftOutput = FFT.InverseDitFFT2D(fftOutput, newWidth, newHeight);
            // Convert the complex numbers back to real numbers, using their magnitudes
            var magnitudes = ifftOutput.Select(n => n.Magnitude()).ToList();
            // We could resize the image again here, but that risks adding more noise.  It can be done later if needed.
            var output = new ImageData { Data = magnitudes.Select(t => (int)t).ToArray(), Width = newWidth, Height = newHeight };

            return output;
        }
    }
}
