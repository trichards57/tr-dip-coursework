using System;
using System.Diagnostics;
using System.IO;
using ManagedDigitalImageProcessing.Filters;

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

            var data = PGM.PgmLoader.LoadImage(inFile);

            var f1 = new GaussianFilter(5);
            var f2 = new SeperatedGaussianFilter(5);

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var output = f1.Filter(data);
            stopwatch.Stop();
            Console.WriteLine("Gauss {0}", stopwatch.ElapsedMilliseconds);

            stopwatch.Restart();
            var output1 = f2.Filter(data);
            stopwatch.Stop();
            Console.WriteLine("Separated Gauss {0}", stopwatch.ElapsedMilliseconds);
            
            output.ToBitmap().Save("Gauss.png");
            output1.ToBitmap().Save("SepGauss.png");
        }
    }
}
