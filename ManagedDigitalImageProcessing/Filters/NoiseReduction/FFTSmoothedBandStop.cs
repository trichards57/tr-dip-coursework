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
    public class FFTSmoothedBandStop : FilterBase
    {
        /// <summary>
        /// The inner position of the stop band
        /// </summary>
        private readonly int inner;

        /// <summary>
        /// The output position of the stop band
        /// </summary>
        private readonly int outer;

        /// <summary>
        /// The length of at either end of the stop band
        /// </summary>
        private readonly int rampLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="FFTSmoothedBandStop"/> class.
        /// </summary>
        /// <param name="inner">The inner position (in pixels) of the stop band.</param>
        /// <param name="outer">The outer position (in pixels) of the stop band.</param>
        /// <param name="rampLength">Length of the ramp.</param>
        public FFTSmoothedBandStop(int inner, int outer, int rampLength)
        {
            this.inner = inner;
            this.outer = outer;
            this.rampLength = rampLength;
        }

        /// <summary>
        /// Applies the band stop to the specified image.
        /// </summary>
        /// <param name="input">The input iamge.</param>
        /// <returns>The band-stop filtered image</returns>
        public ImageData Filter(ImageData input)
        {
            var output = new ImageData { Header = input.Header };
            var width = input.Header.Width;
            var height = input.Header.Height;

            var newWidth = (int)Math.Pow(2, Math.Ceiling(Math.Log(width, 2)));
            var newHeight = (int)Math.Pow(2, Math.Ceiling(Math.Log(height, 2)));

            var resizer = new Resizer(newWidth, newHeight);
            var resizedImage = resizer.Filter(input);

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
                            fftOutput[CalculateIndex(i, j, newWidth, newHeight)] *= Math.Abs(dist - inner) / rampLength;
                        }
                        else if (dist > inner && dist < outer)
                        {
                            fftOutput[CalculateIndex(i, j, newWidth, newHeight)] = 0;
                        }
                        else if (dist > outer && dist < outer + rampLength)
                        {
                            fftOutput[CalculateIndex(i, j, newWidth, newHeight)] *= Math.Abs(dist - outer) / rampLength;
                        }
                    }
                });

            var ifftOutput = FFT.InverseDitFFT2D(fftOutput, newWidth, newHeight);

            var magnitudes = ifftOutput.Select(n => n.Magnitude()).ToList();
            var max = magnitudes.Max();

            output.Data = magnitudes.Select(n => (byte)(255 * n / max)).ToArray();
            output.Header = new ImageHeader { Height = newHeight, Width = newWidth };

            return output;
        }
    }
}
