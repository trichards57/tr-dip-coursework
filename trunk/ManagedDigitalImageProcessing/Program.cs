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

            var medianFilters = new List<FilterBase>();
            var gausFilters = new List<FilterBase>();
            var bitsliceFilters = new List<FilterBase>();
            var edgeDetectors = new List<FilterBase>();

            for (var i = 3; i < 23; i += 2)
                medianFilters.Add(new HistogramMedianFilter(i));
            medianFilters.Add(new NoOpFilter());

            for (var i = 1; i < 12; i++)
                gausFilters.Add(new GaussianFilter(i));
            gausFilters.Add(new NoOpFilter());

            bitsliceFilters.AddRange(new FilterBase[] { new BitwiseAndFilter(0xF0), new NoOpFilter() });
            edgeDetectors.Add(new LaplacianOperator());
            edgeDetectors.Add(new NoOpFilter());
            
            for (byte i = 100; i < 240 && i > 99; i += 25)
                for (byte j = 0; j < 100; j += 10)
                    edgeDetectors.Add(new CannyFilter(i, j));


            var count = 0;

            for (var i = 0; i < medianFilters.Count; i++)
            {
                var output = medianFilters[i].Filter(data);
                for (var j = 0; j < gausFilters.Count; j++)
                {
                    var output1 = gausFilters[j].Filter(output);
                    for (var k = 0; k < bitsliceFilters.Count; k++)
                    {
                        var output2 = bitsliceFilters[k].Filter(output1);
                        for (var l = 0; l < edgeDetectors.Count; l++)
                        {
                            var output3 = edgeDetectors[l].Filter(output2);

                            var image = output3.ToBitmap();
                            var graph = Graphics.FromImage(image);
                            graph.DrawString(string.Format("{0} {1} {2} {3}", medianFilters[i].ToString(), gausFilters[j].ToString(), bitsliceFilters[k].ToString(), edgeDetectors[l].ToString()), SystemFonts.DefaultFont, Brushes.Red, new PointF(10, 10));
                            image.Save(string.Format(@"e:\pictures\pic{0:00000000}.png", count++), ImageFormat.Png);
                        }
                    }
                }
            }

            data.ToBitmap().Save("basic.png", ImageFormat.Png);

            Process.Start("basic.png");
        }
    }
}
