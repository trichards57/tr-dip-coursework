using System;
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
            using (var inFile = File.Open(@"..\..\..\Base Images\NZjers1.pgm", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var data = PgmLoader.LoadImage(inFile);

                //var gauss1 = new GaussianFilter(0.5);
                //var gauss2 = new GaussianFilter(1);
                //var gauss3 = new GaussianFilter(2);
                //var gauss4 = new GaussianFilter(4);
                var gauss5 = new GaussianFilter(8);
                //var gauss6 = new GaussianFilter(16);

                //var out1 = gauss1.Filter(data);
                //var out2 = gauss2.Filter(data);
                //var out3 = gauss3.Filter(data);
                //var out4 = gauss4.Filter(data);
                var out5 = gauss5.Filter(data);
                //var out6 = gauss6.Filter(data);                

                //out1.ToBitmap().Save(string.Format("Gaussian Sigma={0}, Size={1}.png", gauss1.Sigma, gauss1.Size));
                //out2.ToBitmap().Save(string.Format("Gaussian Sigma={0}, Size={1}.png", gauss2.Sigma, gauss2.Size));
                //out3.ToBitmap().Save(string.Format("Gaussian Sigma={0}, Size={1}.png", gauss3.Sigma, gauss3.Size));
                //out4.ToBitmap().Save(string.Format("Gaussian Sigma={0}, Size={1}.png", gauss4.Sigma, gauss4.Size));
                out5.ToBitmap().Save(string.Format("Gaussian Sigma={0}, Size={1}.png", gauss5.Sigma, gauss5.Size));
                //out6.ToBitmap().Save(string.Format("Gaussian Sigma={0}, Size={1}.png", gauss6.Sigma, gauss6.Size));

            }            
        }
    }
}

