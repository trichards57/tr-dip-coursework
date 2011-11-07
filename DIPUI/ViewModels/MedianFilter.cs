using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.Filters;
using ManagedDigitalImageProcessing.Filters.NoiseReduction;
using ManagedDigitalImageProcessing.PGM;

namespace DIPUI.ViewModels
{
    sealed class MedianFilter : Filter
    {
        public MedianFilter() : base("Median Filter", "Window Size")
        {
            FilterFunction = (img =>
                                  {
                                      var output = new PgmImage {Header = img.Header, Data = new byte[img.Data.Length]};
                                      NativeNoiseFilters.HistogramMedianFilter(img.Data.Length, img.Data, output.Data,
                                                                          img.Header.Width, img.Header.Height, Parameter);
                                      return output;
                                  });
        }
    }
}
