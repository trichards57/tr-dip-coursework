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

            var filter = new HistogramMedianFilter(27);
            var output = filter.Filter(data);

            output.ToBitmap().Save("NewMedianFilter.png");
        }
    }
}
