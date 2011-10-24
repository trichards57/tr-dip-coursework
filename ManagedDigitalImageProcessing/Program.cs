using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing.Imaging;
using ManagedDigitalImageProcessing.Filters;
using System.Diagnostics;
using ManagedDigitalImageProcessing.Filters.EdgeDetectors;
using System.Drawing;

namespace ManagedDigitalImageProcessing
{
    /// <summary>
    /// The main class for the program.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The initial function called when the program starts.
        /// </summary>
        /// <param name="args">The arguments presented to the program.</param>
        static void Main(string[] args)
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
