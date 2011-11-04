﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ManagedDigitalImageProcessing.FFT;
using ManagedDigitalImageProcessing.Filters;
using ManagedDigitalImageProcessing.Filters.NoiseReduction;
using ManagedDigitalImageProcessing.Filters.Utilities;
using ManagedDigitalImageProcessing.PGM;
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
            var inFile = File.Open(@"..\..\..\Base Images\foetus.pgm", FileMode.Open, FileAccess.Read, FileShare.Read);
            var data = PgmLoader.LoadImage(inFile);

            var fftBandstopFilter = new FFTBandStop(50, 600);
            var fftSmFilter = new FFTSmoothedBandStop(50, 600, 70);
            var output = fftBandstopFilter.Filter(data);
            var output1 = fftSmFilter.Filter(data);

            output.ToBitmap().Save("Test.png");
            output1.ToBitmap().Save("Test1.png");
        }
    }
}

