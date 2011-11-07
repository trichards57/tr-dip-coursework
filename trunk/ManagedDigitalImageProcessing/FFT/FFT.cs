using System;

namespace ManagedDigitalImageProcessing.FFT
{
    // From http://en.wikipedia.org/wiki/Cooley%E2%80%93Tukey_FFT_algorithm
    static class FFT
    {
        public static ComplexNumber[] DitFFT(ComplexNumber[] x)
        {
            if (x.Length == 1)
                return x;

            var list1 = new ComplexNumber[x.Length / 2];
            var list2 = new ComplexNumber[x.Length / 2];

            for (var i = 0; i < x.Length; i++)
            {
                if (i % 2 == 0)
                    list1[i / 2] = x[i];
                else
                    list2[i / 2] = x[i];
            }

            list1 = DitFFT(list1);
            list2 = DitFFT(list2);

            var output = new ComplexNumber[list1.Length + list2.Length];
            for (var i = 0; i < list1.Length; i++)
            {
                var t = list1[i];
                output[i] = t +
                            new ComplexNumber(Math.Cos(2 * Math.PI * i / (list1.Length * 2)),
                                              -Math.Sin(2 * Math.PI * i / (list1.Length * 2))) * list2[i];
                output[i + list1.Length] = t - new ComplexNumber(Math.Cos(2 * Math.PI * i / (list1.Length * 2)),
                                                                -Math.Sin(2 * Math.PI * i / (list1.Length * 2))) * list2[i];


            }
            return output;
        }

        private static ComplexNumber[] InverseDitFFT(ComplexNumber[] x)
        {
            var swappedList = new ComplexNumber[x.Length];
            for (var i = 0; i < x.Length; i++)
                swappedList[i] = x[i].Swap();

            var outputTemp = DitFFT(swappedList);

            swappedList = new ComplexNumber[outputTemp.Length];
            for (var i = 0; i < outputTemp.Length; i++)
                swappedList[i] = outputTemp[i].Swap();

            return swappedList;
        }

        private static ComplexNumber[] DitFFT2D(ComplexNumber[] x, int width, int height, bool centerOutput, Func<ComplexNumber[], ComplexNumber[]> fftOp)
        {
            var buffer = new ComplexNumber[x.Length];

            if (centerOutput)
            {
                for (var i = 0; i < width; i++)
                {
                    for (var j = 0; j < height; j++)
                    {
                        buffer[i + j * width] = x[i + j * width] * Math.Pow(-1, i + j);
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
                var buf = new ComplexNumber[width];
                Array.Copy(buffer, i * width, buf, 0, width);
                var fftOutput = fftOp(buf);
                Array.Copy(fftOutput, 0, outputTemp, i * width, width);
            }
            var output = new ComplexNumber[width * height];
            for (var i = 0; i < width; i++)
            {
                var buf = new ComplexNumber[height];
                for (var j = 0; j < height; j++)
                {
                    var index = i + j * width;
                    buf[j] = outputTemp[index];
                }

                var colOut = fftOp(buf);
                for (var j = 0; j < height; j++)
                {
                    output[i + j * width] = colOut[j];
                }
            }

            return output;
        }

        public static ComplexNumber[] DitFFT2D(ComplexNumber[] x, int width, int height)
        {
            return DitFFT2D(x, width, height, true, DitFFT);
        }

        public static ComplexNumber[] InverseDitFFT2D(ComplexNumber[] x, int width, int height)
        {
            return DitFFT2D(x, width, height, false, InverseDitFFT);
        }
    }
}
