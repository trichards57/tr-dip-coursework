namespace ManagedDigitalImageProcessing.FFT
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Static class, used to perform an Fast Fourier Transform or Inverse Fast Fourier Transform on data.
    /// </summary>
    public static class FFT
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
            // Calls the core FFT algorithm, using the DitFFT funtion to perform the transform.
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
            // Calls the core FFT algorithm, using the InverseDitFFT function to perform the transform.
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
            Parallel.For(
                0,
                x.Length,
                i =>
                    {
                        if (i % 2 == 0)
                        {
                            list1[i / 2] = x[i];
                        }
                        else
                        {
                            list2[i / 2] = x[i];
                        }
                    });

            // Run the FFT on each one
            var outlist1 = DitFFT(list1);
            var outlist2 = DitFFT(list2);

            // Multiply the results back together.
            var output = new ComplexNumber[outlist1.Length + outlist2.Length];
            Parallel.For(
                0,
                outlist1.Length,
                i =>
                    {
                        var t = outlist1[i];
                        var twoPiI = 2 * Math.PI * i / (outlist1.Length * 2);
                        output[i] = t + (new ComplexNumber(Math.Cos(twoPiI), -Math.Sin(twoPiI)) * outlist2[i]);
                        output[i + outlist1.Length] = t
                                                      -
                                                      (new ComplexNumber(Math.Cos(twoPiI), -Math.Sin(twoPiI))
                                                       * outlist2[i]);
                    });

            return output;
        }

        /// <summary>
        /// Performs the 1D Inverse Fast Fourier Transfor on the data.
        /// </summary>
        /// <param name="x">The data to perform the transform on.</param>
        /// <returns>The IFFT transformed data.</returns>
        /// <remarks>
        /// Algorithm taken from @cite dftWikipedia
        /// </remarks>
        private static ComplexNumber[] InverseDitFFT(ComplexNumber[] x)
        {
            // Swap the real and imaginary components of the complex numbers to perform the FFT.
            var swappedList = x.AsParallel().AsOrdered().Select(i => i.Swap()).ToArray();

            var output = DitFFT(swappedList);

            return output.AsParallel().AsOrdered().Select(i => i.Swap()).ToArray();
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
                // To move the 0 Hz point to the center of the image, multiple each pixel by -1^(x+y)
                Parallel.For(
                    0,
                    width,
                    i =>
                        {
                            for (var j = 0; j < height; j++)
                            {
                                buffer[i + (j * width)] = x[i + (j * width)] * Math.Pow(-1, i + j);
                            }
                        });
            }
            else
            {
                // If the output doesn't need to be centered, just copy the image as is in to the buffer.
                Array.Copy(x, buffer, x.Length);
            }

            var outputTemp = new ComplexNumber[x.Length];

            Parallel.For(
                0,
                height,
                i =>
                    {
                        // Perform the a 1D FFT on each line.
                        var buf = new ComplexNumber[width];
                        Array.Copy(buffer, i * width, buf, 0, width);
                        var fftOutput = fftOp(buf);
                        Array.Copy(fftOutput, 0, outputTemp, i * width, width);
                    });

            var output = new ComplexNumber[width * height];
            Parallel.For(
                0,
                width,
                i =>
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
                    });

            return output;
        }
    }
}
