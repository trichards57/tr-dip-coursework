using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagedDigitalImageProcessing.FFT
{
    // From http://en.wikipedia.org/wiki/Cooley%E2%80%93Tukey_FFT_algorithm
    class FFT
    {
        protected static int CalculateIndex(int x, int y, int width, int height)
        {
            if (x < 0)
                x = -x;
            if (x >= width)
                x = width - (x - width) - 1;

            if (y < 0)
                y = -y;
            if (y >= height)
                y = height - (y - height) - 1;

            return x + y * width;
        }

        public static List<ComplexNumber> DitFFT(List<ComplexNumber> x)
        {
            if (x.Count == 1)
                return x;
            var list1 = DitFFT(x.Where((c, i) => i % 2 == 0).ToList());
            var list2 = DitFFT(x.Where((c, i) => i % 2 == 1).ToList());
            var output = new ComplexNumber[list1.Count + list2.Count];
            for (var i = 0; i < list1.Count; i++)
            {
                var t = list1[i];
                output[i] = t +
                            new ComplexNumber(Math.Cos(2 * Math.PI * i / (list1.Count * 2)),
                                              -Math.Sin(2 * Math.PI * i / (list1.Count * 2))) * list2[i];
                output[i + list1.Count] = t - new ComplexNumber(Math.Cos(2 * Math.PI * i / (list1.Count * 2)),
                                                                -Math.Sin(2 * Math.PI * i / (list1.Count * 2))) * list2[i];


            }
            return output.ToList();
        }

        public static List<ComplexNumber> InverseDitFFT(List<ComplexNumber> x)
        {
            var swappedList = x.Select(n => n.Swap()).ToList();

            var outputTemp = DitFFT(swappedList);
            return outputTemp.Select(n => n.Swap()).ToList();
        }

        public static List<ComplexNumber> DitFFT2D(List<ComplexNumber> x, int width, int height)
        {
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    x[i + j * 1024] *= Math.Pow(-1, i + j);
                }
            }


            var outputTemp = new List<ComplexNumber>();
            for (var i = 0; i < height; i++)
            {
                outputTemp.AddRange(DitFFT(x.Skip(i * width).Take(width).ToList()));
            }
            var output = new ComplexNumber[width * height];
            for (var i = 0; i < width; i++)
            {
                var colIndex = i;
                var col = outputTemp.Where((val, index) => index%width == colIndex).ToList();
                var colOut = DitFFT(col);
                for (var j = 0; j < height; j++)
                {
                    output[i + j*width] = colOut[j];
                }
            }

            return output.ToList();
        }

        public static List<ComplexNumber> InverseDitFFT2D(List<ComplexNumber> x, int width, int height)
        {
            var outputTemp = new List<ComplexNumber>();
            for (var i = 0; i < height; i++)
            {
                outputTemp.AddRange(InverseDitFFT(x.Skip(i * width).Take(width).ToList()));
            }
            var output = new ComplexNumber[width * height];
            for (var i = 0; i < width; i++)
            {
                var colIndex = i;
                var col = outputTemp.Where((val, index) => index % width == colIndex).ToList();
                var colOut = InverseDitFFT(col);
                for (var j = 0; j < height; j++)
                {
                    output[i + j * width] = colOut[j];
                }
            }

            return output.ToList();
        }
    }
}
