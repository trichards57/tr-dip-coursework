using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing.Imaging;
using ManagedDigitalImageProcessing.Filters;
using System.Diagnostics;
using ManagedDigitalImageProcessing.Filters.EdgeDetectors;

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

            var noiseFilter = new GaussianFilter(3, 0.5);
            var edgeFilter = new SobelOperator();
            var bitsliceFilter = new BitwiseAndFilter(0xC0);
            var nonMaximal = new NonMaximumSuppression();
            var hysteresis = new HysteresisThresholding(180, 10);
            var laplaceFilter = new LaplacianOperator();

            var output = noiseFilter.Filter(data);
            //output = bitsliceFilter.Filter(output);
            //output = hysteresis.Filter(nonMaximal.Filter(edgeFilter.FilterSplit(output)).ToPgmImage());
            output = laplaceFilter.Filter(output);

            data.ToBitmap().Save("basic.png", ImageFormat.Png);
            output.ToBitmap().Save("output.png", ImageFormat.Png);
        }
    }
}
