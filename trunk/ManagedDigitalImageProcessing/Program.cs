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
            NativeMorphologicalOperators.Erode(data);
            //var filter = new Close(15);

            var stopwatch = new Stopwatch();
            var testNumber = 1;

            var outImage = new PgmImage {Header = data.Header};

            stopwatch.Start();
            for (var i = 0; i < testNumber; i++)
                outImage = NativeMorphologicalOperators.Gradient(data);
            stopwatch.Stop();

            outImage.Data = outputData;
            outImage.ToBitmap().Save("Test1.png");

            Console.WriteLine("Unmanaged Histogram : {0}", (double)stopwatch.ElapsedMilliseconds / testNumber);

            stopwatch.Restart();
            //outImage = filter.Filter(data);
            stopwatch.Stop();

            outImage.ToBitmap().Save("Test2.png");

            Console.WriteLine("Managed Histogram   : {0}", (double)stopwatch.ElapsedMilliseconds / testNumber);

            Console.ReadLine();
        }
    }
}

