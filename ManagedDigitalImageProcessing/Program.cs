using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing.Imaging;
using ManagedDigitalImageProcessing.Filters;

namespace ManagedDigitalImageProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            var inFile = File.Open(@"..\..\..\Base Images\NZjers1.pgm", FileMode.Open, FileAccess.Read, FileShare.Read);

            var data = PGM.PgmLoader.LoadImage(inFile);

            for (var i = 3; i < 26; i+=2)
            {
                var filter = new MedianFilter(i);
                var filteredData = filter.Filter(data);
                filteredData.ToBitmap().Save(string.Format("medianFilteredTest{0:00}.png",i), ImageFormat.Png);
            }

            for (var i = 3; i < 26; i += 2)
            {
                var filter = new CrossMedianFilter(i);
                var filteredData = filter.Filter(data);
                filteredData.ToBitmap().Save(string.Format("crossFilteredTest{0:00}.png", i), ImageFormat.Png);
            }

            data.ToBitmap().Save("basic.png", ImageFormat.Png);
            
        }
    }
}
