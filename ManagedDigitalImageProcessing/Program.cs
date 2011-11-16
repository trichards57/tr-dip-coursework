using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using ManagedDigitalImageProcessing.FFT;
using ManagedDigitalImageProcessing.Filters;
using ManagedDigitalImageProcessing.Filters.NoiseReduction;
using ManagedDigitalImageProcessing.Filters.Utilities;
using ManagedDigitalImageProcessing.Images;
using ManagedDigitalImageProcessing.Filters.EdgeDetectors;

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
            var sobel = new SobelOperator();
            var median = new AdaptiveHistogramMedianFilter(99, 0.5, 13);

            using (var inFile = Image.FromFile(@"..\..\..\Base Images\roxy.png") as Bitmap)
            {
                var data = PgmLoader.LoadBitmap(inFile);
                var output = sobel.Filter(median.Filter(data));
                output.ToBitmap().Save("Roxy Sobel.png");

                //var median = new[]
                //                 {
                //                     new AdaptiveHistogramMedianFilter(99, 0.5, 3),
                //                     new AdaptiveHistogramMedianFilter(99, 0.5, 5),
                //                     new AdaptiveHistogramMedianFilter(99, 0.5, 7),
                //                     new AdaptiveHistogramMedianFilter(99, 0.5, 9),
                //                     new AdaptiveHistogramMedianFilter(99, 0.5, 11),
                //                     new AdaptiveHistogramMedianFilter(99, 0.5, 13)
                //                 };
                //var output = median.Select(g => new { Filter = g, Data = g.Filter(data) });

                //foreach (var b in output.Select(o => new { o.Filter, Bitmap = o.Data.ToBitmap() }))
                //{
                //    b.Bitmap.Save(string.Format("Noisy Roxy Adaptive Median Size={0}.png", b.Filter.Size));
                //}
            }
            using (var inFile = File.OpenRead(@"..\..\..\Base Images\foetus.pgm"))
            {
                var data = PgmLoader.LoadImage(inFile);
                var output = sobel.Filter(median.Filter(data));
                output.ToBitmap().Save("Foetus Sobel.png");
                
                //var median = new[]
                //                 {
                //                     new AdaptiveHistogramMedianFilter(99, 0.5, 3),
                //                     new AdaptiveHistogramMedianFilter(99, 0.5, 5),
                //                     new AdaptiveHistogramMedianFilter(99, 0.5, 7),
                //                     new AdaptiveHistogramMedianFilter(99, 0.5, 9),
                //                     new AdaptiveHistogramMedianFilter(99, 0.5, 11),
                //                     new AdaptiveHistogramMedianFilter(99, 0.5, 13)
                //                 };
                //var output = median.Select(g => new { Filter = g, Data = g.Filter(data) });

                //foreach (var b in output.Select(o => new { o.Filter, Bitmap = o.Data.ToBitmap() }))
                //{
                //    b.Bitmap.Save(string.Format("Foetus Adaptive Median Size={0}.png", b.Filter.Size));
                //}
            }
            using (var inFile = File.OpenRead(@"..\..\..\Base Images\NZjers1.pgm"))
            {
                var data = PgmLoader.LoadImage(inFile);
                var output = sobel.Filter(median.Filter(data));
                output.ToBitmap().Save("SAR Sobel.png");

                //var median = new[]
                //                 {
                //                     new AdaptiveHistogramMedianFilter(99, 0.5, 3),
                //                     new AdaptiveHistogramMedianFilter(99, 0.5, 5),
                //                     new AdaptiveHistogramMedianFilter(99, 0.5, 7),
                //                     new AdaptiveHistogramMedianFilter(99, 0.5, 9),
                //                     new AdaptiveHistogramMedianFilter(99, 0.5, 11),
                //                     new AdaptiveHistogramMedianFilter(99, 0.5, 13)
                //                 };
                //var output = median.Select(g => new { Filter = g, Data = g.Filter(data) });

                //foreach (var b in output.Select(o => new { o.Filter, Bitmap = o.Data.ToBitmap() }))
                //{
                //    b.Bitmap.Save(string.Format("SAR Adaptive Median Size={0}.png", b.Filter.Size));
                //}
            }
        }
    }
}

