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

            NativeFilter.BitwiseAndFilter(data.Data, data.Data.Length, 0xF0);
        }
    }
}
