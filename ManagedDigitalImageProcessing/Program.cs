using System;
using System.Diagnostics;
using System.IO;
using ManagedDigitalImageProcessing.FFT;
using ManagedDigitalImageProcessing.Filters.NoiseReduction;
using ManagedDigitalImageProcessing.PGM;
using System.Drawing;

namespace ManagedDigitalImageProcessing
{
    /// <summary>
    /// The main class for the program.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The initial function called when the program starts.
        /// </summary>
        static void Main()
        {
            //var inFile = File.Open(@"..\..\..\Base Images\foetus.pgm", FileMode.Open, FileAccess.Read, FileShare.Read);
            //var data = PgmLoader.LoadImage(inFile);

            var bitmap = Bitmap.FromFile(@"..\..\..\Base Images\roxySaltAndPepper.png") as Bitmap;

            var newData = PgmLoader.LoadBitmap(bitmap);

            var canny = new MorphologicalGradient(7);
            var gauss = new HistogramMedianFilter(3);

            var output = gauss.Filter(newData);

            output.ToBitmap().Save("testRoxy.png");

            //var fftBandstopFilter = new FFTBandStop(50, 600);
            //var fftSmFilter = new FFTSmoothedBandStop(50, 600, 70);
            //var output = fftBandstopFilter.Filter(data);
            //var output1 = fftSmFilter.Filter(data);

            //output.ToBitmap().Save("Test.png");
            //output1.ToBitmap().Save("Test1.png");
        }
    }
}

