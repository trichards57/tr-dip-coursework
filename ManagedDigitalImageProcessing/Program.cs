// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Tony Richards">
//   Copyright (c) 2011, Tony Richards
//   All rights reserved.
//
//   Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
//   following conditions are met:
//
//   Redistributions of source code must retain the above copyright notice, this list of conditions and the following 
//   disclaimer.
//   
//   Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the 
//   following disclaimer in the documentation and/or other materials provided with the distribution.
//   
//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
//   INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
//   DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
//   SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
//   SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//   WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE 
//   USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// <summary>
//   The main class for the program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ManagedDigitalImageProcessing
{
    using System.IO;

    using ManagedDigitalImageProcessing.Filters.EdgeDetectors;
    using ManagedDigitalImageProcessing.Filters.NoiseReduction;
    using ManagedDigitalImageProcessing.Images;

    /// <summary>
    /// The main class for the program.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The initial function called when the program starts.
        /// </summary>
        public static void Main()
        {
            var med = new HistogramMedianFilter(21);
            var sobel = new SobelOperator();
            var canny = new CannyFilter(40, 39);

            using (var inFile = File.OpenRead(@"..\..\..\Base Images\foetus.pgm"))
            {
                var data = ImageLoader.LoadPgmImage(inFile);
                var output1 = med.Filter(data);
                var output2 = canny.Filter(output1);
                var output3 = SobelOperator.Filter(output1);

                output1.ToBitmap().Save("Median.png");
                output2.ToBitmap().Save("Canny.png");
                output3.ToBitmap().Save("Sobel.png");
            }
        }
    }
}