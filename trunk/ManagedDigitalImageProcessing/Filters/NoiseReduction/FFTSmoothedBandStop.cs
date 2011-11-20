// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FFTSmoothedBandStop.cs" company="Tony Richards">
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
//   Defines the FFTSmoothedBandStop type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ManagedDigitalImageProcessing.FFT;
    using ManagedDigitalImageProcessing.Filters.Utilities;
    using ManagedDigitalImageProcessing.Images;

    /// <summary>
    ///  Filter class which applies a band stop filter (with ramped edges) using FFT techniques.
    /// </summary>
    public static class FFTSmoothedBandStop
    {
        /// <summary>
        /// Applies the band stop to the specified image.
        /// </summary>
        /// <param name="input">The input iamge.</param>
        /// <param name="inner">The inner edge of the bandstop.</param>
        /// <param name="outer">The outer edge of the bandstop.</param>
        /// <param name="rampLength">Length of the bandstop edge ramp.</param>
        /// <returns>
        /// The band-stop filtered image
        /// </returns>
        public static ImageData Filter(ImageData input, int inner, int outer, int rampLength)
        {
            var width = input.Width;
            var height = input.Height;

            var newWidth = (int)Math.Pow(2, Math.Ceiling(Math.Log(width, 2)));
            var newHeight = (int)Math.Pow(2, Math.Ceiling(Math.Log(height, 2)));

            var resizedImage = Resizer.Filter(input, newWidth, newHeight);

            var complexInputData = new ComplexNumber[newWidth * newHeight];
            Parallel.For(0, newWidth * newHeight, i => complexInputData[i] = resizedImage.Data[i]);

            var fftOutput = FFT.DitFFT2D(complexInputData, newWidth, newHeight);

            var centerX = (newWidth / 2.0) + 0.5;
            var centerY = (newHeight / 2.0) + 0.5;

            Parallel.For(
                0,
                newWidth,
                i =>
                {
                    for (var j = 0; j < newHeight; j++)
                    {
                        var distX = i - centerX;
                        var distY = j - centerY;
                        var dist = Math.Sqrt((distX * distX) + (distY * distY));
                        if (dist < inner && dist > inner - rampLength)
                        {
                            fftOutput[ImageUtilities.CalculateIndex(i, j, newWidth)] *= Math.Abs(dist - inner) / rampLength;
                        }
                        else if (dist > inner && dist < outer)
                        {
                            fftOutput[ImageUtilities.CalculateIndex(i, j, newWidth)] = 0;
                        }
                        else if (dist > outer && dist < outer + rampLength)
                        {
                            fftOutput[ImageUtilities.CalculateIndex(i, j, newWidth)] *= Math.Abs(dist - outer) / rampLength;
                        }
                    }
                });

            var ifftOutput = FFT.InverseDitFFT2D(fftOutput, newWidth, newHeight);

            var magnitudes = ifftOutput.Select(n => n.Magnitude()).ToList();

            var output = new ImageData { Data = magnitudes.Select(t => (int)t).ToArray(), Height = newHeight, Width = newWidth };

            return output;
        }
    }
}
