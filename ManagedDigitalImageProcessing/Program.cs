using System;
using System.Diagnostics;
using System.IO;
using ManagedDigitalImageProcessing.Filters;
using ManagedDigitalImageProcessing.PGM;

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
            var filter = new BitwiseAndFilter(0xF0);



            var length = data.Data.Length;
            var paddedLength = length + (16 - (length % 16)); // Increased so that the data fits perfectly in to the SSE2 registers.
            var bytes = new byte[paddedLength];
            Array.Copy(data.Data, bytes, length);

            var stopwatch = new Stopwatch();

            stopwatch.Start();
            for (var i = 0; i < 50; i++)
                NativeFilter.BitwiseAndFilter(paddedLength, bytes, 0xF0);

            stopwatch.Stop();

            var newStopwatch = new Stopwatch();
            newStopwatch.Start();
            for (var i = 0; i < 50; i++)
                filter.Filter(data);
            newStopwatch.Stop();

            Console.WriteLine("Managed Function : {0}", newStopwatch.ElapsedMilliseconds);
            Console.WriteLine("Unmanaged Function : {0}", stopwatch.ElapsedMilliseconds);
            Console.ReadLine();
        }
    }
}
