using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ManagedDigitalImageProcessing.FFT;
using ManagedDigitalImageProcessing.Filters;
using ManagedDigitalImageProcessing.Filters.NoiseReduction;
using ManagedDigitalImageProcessing.Filters.Utilities;
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

            var adaptFilter = new AdaptiveHistogramMedianFilter(99, 0.5, 15);
            var histMedFilter = new HistogramMedianFilter(15);
            var medFilter = new MedianFilter(15);

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var output1 = medFilter.Filter(data);
            stopwatch.Stop();

            Console.WriteLine("Managed : {0}", stopwatch.ElapsedMilliseconds);

            stopwatch.Restart();
            var outputData = new byte[data.Data.Length];
            
            stopwatch.Stop();

            Console.WriteLine("Unmanaged : {0}", stopwatch.ElapsedMilliseconds);

            output1.ToBitmap().Save("Test1.png");
            output1.Data = outputData;
            output1.ToBitmap().Save("Test2.png");
        }
    }
}

