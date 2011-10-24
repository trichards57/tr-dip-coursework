using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.Filters;
using ManagedDigitalImageProcessing.PGM;

namespace TimingTest
{
    class Program
    {
        static void Main(string[] args)
        {
            const int cycleCount = 10;

            var stopwatch = new Stopwatch();

            var inFile = File.Open(@"..\..\..\Base Images\foetus.pgm", FileMode.Open, FileAccess.Read, FileShare.Read);

            var data = PgmLoader.LoadImage(inFile);
            var gausFilter = new GaussianFilter(2);
            var sepGausFilter = new SeperatedGaussianFilter(2);
            var median = new MedianFilter(21);
            var histogramMedian = new HistogramMedianFilter(21);

            stopwatch.Start();
            for (var i = 0; i < cycleCount; i++)
            {
                gausFilter.Filter(data);
            }
            stopwatch.Stop();

            Console.WriteLine("Gaussian Filter : {0}", stopwatch.ElapsedMilliseconds / cycleCount);

            stopwatch.Restart();
            for (var i = 0; i < cycleCount; i++)
            {
                sepGausFilter.Filter(data);
            }
            stopwatch.Stop();

            Console.WriteLine("Seperated Gaussian Filter : {0}", stopwatch.ElapsedMilliseconds / cycleCount);

            stopwatch.Restart();
            for (var i = 0; i < cycleCount; i++)
            {
                median.Filter(data);
            }
            stopwatch.Stop();

            Console.WriteLine("Median Filter : {0}", stopwatch.ElapsedMilliseconds / cycleCount);

            stopwatch.Restart();
            for (var i = 0; i < cycleCount; i++)
            {
                histogramMedian.Filter(data);
            }
            stopwatch.Stop();

            Console.WriteLine("Histogram Median Filter : {0}", stopwatch.ElapsedMilliseconds / cycleCount);
        }
    }
}
