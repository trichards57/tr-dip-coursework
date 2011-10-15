using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing.Imaging;
using ManagedDigitalImageProcessing.Filters;
using System.Diagnostics;

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

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (var i = 3; i < 26; i += 2)
            {
                var filter = new MedianFilter(i);
                var filteredData = filter.Filter(data);
                filteredData.ToBitmap().Save(string.Format("medianFilteredTest{0:00}.png", i), ImageFormat.Png);
            }

            stopwatch.Stop();
            Console.WriteLine("Median Filter Time : {0:0.0000}", stopwatch.Elapsed.TotalSeconds);

            stopwatch.Restart();

            for (var i = 3; i < 26; i += 2)
            {
                var filter = new HistogramMedianFilter(i);
                var filteredData = filter.Filter(data);
                filteredData.ToBitmap().Save(string.Format("histogramMedianFilteredTest{0:00}.png", i), ImageFormat.Png);
            }

            stopwatch.Stop();
            Console.WriteLine("Histogram Median Filter Time : {0:0.0000}", stopwatch.Elapsed.TotalSeconds);

            for (var i = 3; i < 26; i += 2)
            {
                var filter = new CrossMedianFilter(i);
                var filteredData = filter.Filter(data);
                filteredData.ToBitmap().Save(string.Format("crossFilteredTest{0:00}.png", i), ImageFormat.Png);
            }

            for (var i = 3; i < 26; i += 2)
            {
                var filter = new TruncatedMedianFilter(i);
                var filteredData = filter.Filter(data);
                filteredData.ToBitmap().Save(string.Format("truncatedMedianFilteredTest{0:00}.png", i), ImageFormat.Png);
            }

            data.ToBitmap().Save("basic.png", ImageFormat.Png);

        }
    }
}
