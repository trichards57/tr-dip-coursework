using System;
using System.Diagnostics;
using System.IO;
using ManagedDigitalImageProcessing.Filters.NoiseReduction;
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

            var outputData = new byte[data.Data.Length];
            NativeFilters.MedianFilter(data.Data.Length, data.Data, outputData, data.Header.Width,
                                                       data.Header.Height, 11);

            var histFilter = new AdaptiveHistogramMedianFilter(99, 0.5, 21);
            
            var stopwatch = new Stopwatch();
            var testNumber = 1;

            var outImage = new PgmImage {Header = data.Header};

            stopwatch.Start();
            for (var i = 0; i < testNumber; i++)
                NativeFilters.AdaptiveHistogramMedianFilter(data.Data.Length, data.Data, outputData, data.Header.Width,
                                                       data.Header.Height, 11, 99, 0.5);
            stopwatch.Stop();

            outImage.Data = outputData;
            outImage.ToBitmap().Save("Test1.png");

            Console.WriteLine("Unmanaged Histogram : {0}", (double)stopwatch.ElapsedMilliseconds / testNumber);

            stopwatch.Restart();
            for (var i = 0; i < testNumber; i++)
                outImage = histFilter.Filter(data);
            stopwatch.Stop();

            //outImage.Data = outputData;
            outImage.ToBitmap().Save("Test2.png");

            Console.WriteLine("Managed Histogram   : {0}", (double)stopwatch.ElapsedMilliseconds / testNumber);

            Console.ReadLine();
        }
    }
}

