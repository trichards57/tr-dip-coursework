// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FFT.cs" company="Tony Richards">
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
//   Defines the FFT type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing.FFT
{
    using System;

    /// <summary>
    /// Static class, used to perform an Fast Fourier Transform or Inverse Fast Fourier Transform on data.
    /// </summary>
    internal static class FFT
    {
        /// <summary>
        /// Performs the 2D Fast Fourier Transform on a set of complex number data.
        /// </summary>
        /// <param name="x">The data to transform.</param>
        /// <param name="width">The width of the data.</param>
        /// <param name="height">The height of the data.</param>
        /// <returns>The FFT transformed data.</returns>
        public static ComplexNumber[] DitFFT2D(ComplexNumber[] x, int width, int height)
        {
            return DitFFT2D(x, width, height, true, DitFFT);
        }

        /// <summary>
        /// Performs the 2D  Inverse Fast Fourier Transform on a set of complex number data.
        /// </summary>
        /// <param name="x">The data to transform.</param>
        /// <param name="width">The width of the data.</param>
        /// <param name="height">The height of the data.</param>
        /// <returns>The FFT transformed data.</returns>
        public static ComplexNumber[] InverseDitFFT2D(ComplexNumber[] x, int width, int height)
        {
            return DitFFT2D(x, width, height, false, InverseDitFFT);
        }

        /// <summary>
        /// Computes the 1D Fast Fourier Transform on the complex number data.
        /// </summary>
        /// <param name="x">The data to perform the transform on.</param>
        /// <returns>The FFT transformed data.</returns>
        /// <remarks>
        /// Code produced from pseudocode from @cite fftAlgorithm
        /// </remarks>
        private static ComplexNumber[] DitFFT(ComplexNumber[] x)
        {
            // The simple case, and the end case of the recursion.
            // The FFT of a single number is itself.
            if (x.Length == 1)
            {
                return x;
            }

            var list1 = new ComplexNumber[x.Length / 2];
            var list2 = new ComplexNumber[x.Length / 2];

            // Partition the list into two interleaved lists.
            for (var i = 0; i < x.Length; i++)
            {
                if (i % 2 == 0)
                {
                    list1[i / 2] = x[i];
                }
                else
                {
                    list2[i / 2] = x[i];
                }
            }

            // Run the FFT on each one
            list1 = DitFFT(list1);
            list2 = DitFFT(list2);

            // Multiply the results back together.
            var output = new ComplexNumber[list1.Length + list2.Length];
            for (var i = 0; i < list1.Length; i++)
            {
                var t = list1[i];
                output[i] = t + (new ComplexNumber(
                                Math.Cos(2 * Math.PI * i / (list1.Length * 2)),
                                     -Math.Sin(2 * Math.PI * i / (list1.Length * 2))) * list2[i]);
                output[i + list1.Length] = t - (new ComplexNumber(
                                                Math.Cos(2 * Math.PI * i / (list1.Length * 2)),
                                                -Math.Sin(2 * Math.PI * i / (list1.Length * 2))) * list2[i]);
            }

            return output;
        }

        /// <summary>
        /// Performs the 1D Inverse Fast Fourier Transfor on the data.
        /// </summary>
        /// <param name="x">The data to perform the transform on.</param>
        /// <returns>The IFFT transformed data.</returns>
        /// @todo Find the citation for this...
        private static ComplexNumber[] InverseDitFFT(ComplexNumber[] x)
        {
            var swappedList = new ComplexNumber[x.Length];
            for (var i = 0; i < x.Length; i++)
            {
                swappedList[i] = x[i].Swap();
            }

            var outputTemp = DitFFT(swappedList);

            swappedList = new ComplexNumber[outputTemp.Length];
            for (var i = 0; i < outputTemp.Length; i++)
            {
                swappedList[i] = outputTemp[i].Swap();
            }

            return swappedList;
        }

        /// <summary>
        /// Performs the 2D Fast Fourier Transform on a set of complex number data.
        /// </summary>
        /// <param name="x">The data to transform.</param>
        /// <param name="width">The width of the data.</param>
        /// <param name="height">The height of the data.</param>
        /// <param name="centerOutput">if set to <c>true</c>, transform the data to center the FFT output..</param>
        /// <param name="fftOp">The 1D FFT operator to use.</param>
        /// <returns>The FFT transformed data.</returns>
        /// <remarks>
        /// <para>Performs the 2D FFT by first performing the FFT on each row individually, and then on each resulting column individually.</para>
        /// <para>To perform an FFT, set <paramref name="fftOp"/> to <see cref="DitFFT"/>. To perform an IFFT, set <paramref name="fftOp"/>
        /// to <see cref="InverseDitFFT"/></para>
        /// <para>Code produced from the algorithm described on @cite visionLectureNotes.</para>
        /// <para>FFT centering code produced from the algorithm described in @cite lectures</para>
        /// </remarks>
        private static ComplexNumber[] DitFFT2D(ComplexNumber[] x, int width, int height, bool centerOutput, Func<ComplexNumber[], ComplexNumber[]> fftOp)
        {
            var buffer = new ComplexNumber[x.Length];

            if (centerOutput)
            {
                // To move the 0 Hz point to the center of the image, multiple each pixel by -1^(i+j)
                for (var i = 0; i < width; i++)
                {
                    for (var j = 0; j < height; j++)
                    {
                        buffer[i + (j * width)] = x[i + (j * width)] * Math.Pow(-1, i + j);
                    }
                }
            }
            else
            {
                Array.Copy(x, buffer, x.Length);
            }

            var outputTemp = new ComplexNumber[x.Length];

            for (var i = 0; i < height; i++)
            {
                // Perform the a 1D FFT on each line.
                var buf = new ComplexNumber[width];
                Array.Copy(buffer, i * width, buf, 0, width);
                var fftOutput = fftOp(buf);
                Array.Copy(fftOutput, 0, outputTemp, i * width, width);
            }

            var output = new ComplexNumber[width * height];
            for (var i = 0; i < width; i++)
            {
                // Copy a column into a temporary buffer
                var buf = new ComplexNumber[height];
                for (var j = 0; j < height; j++)
                {
                    var index = i + (j * width);
                    buf[j] = outputTemp[index];
                }

                // Run the FFT on each of the columns
                var colOut = fftOp(buf);

                // Copy the column into the output
                for (var j = 0; j < height; j++)
                {
                    output[i + (j * width)] = colOut[j];
                }
            }

            return output;
        }
    }
}
