using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ManagedDigitalImageProcessing.FFT;
using ManagedDigitalImageProcessing.Filters;
using ManagedDigitalImageProcessing.PGM;

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
            var inFile = File.Open(@"..\..\..\Base Images\foetus.pgm", FileMode.Open, FileAccess.Read, FileShare.Read);

            var data = PgmLoader.LoadImage(inFile);

            var sizer = new Resizer(1024, 512);
            var output = sizer.Filter(data);

            output.ToBitmap().Save("ResizedOutput.png");
            data.ToBitmap().Save("OriginalImage.png");

            var complexData = output.Data.Select(c => (ComplexNumber)c).ToList();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var outputData = FFT.FFT.DitFFT2D(complexData, 1024, 512);
            stopwatch.Stop();
            Console.WriteLine("Serial FFT : {0}", stopwatch.ElapsedMilliseconds);

            stopwatch.Restart();
            var testData = FFT.FFT.InverseDitFFT2D(outputData, 1024, 512);
            stopwatch.Stop();
            Console.WriteLine("Serial IFFT : {0}", stopwatch.ElapsedMilliseconds);

            stopwatch.Restart();
            outputData = ParallelFFT.DitFFT2D(complexData, 1024, 512);
            stopwatch.Stop();
            Console.WriteLine("Parallel FFT : {0}", stopwatch.ElapsedMilliseconds);

            stopwatch.Restart();
            testData = ParallelFFT.InverseDitFFT2D(outputData, 1024, 512);
            stopwatch.Stop();
            Console.WriteLine("Parallel IFFT : {0}", stopwatch.ElapsedMilliseconds);


            var newData = outputData.Select(c => Math.Log(c.Magnitude())).ToArray();
            var newTestData = testData.Select(c => c.Magnitude()).ToArray();

            var max = newData.Max();
            var testMax = newTestData.Max();

            var scaledNewData = newData.Select(i => (byte)(i * 255.0 / max)).ToArray();
            var scaledTestData = newTestData.Select(i => (byte) (i*255.0/testMax)).ToArray();

            output.Data = scaledNewData;
            output.ToBitmap().Save("FFTOutput.png");

            output.Data = scaledTestData;
            output.ToBitmap().Save("IFFTOutput.png");
        }
    }
}
