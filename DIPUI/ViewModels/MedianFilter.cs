using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.Filters;
using ManagedDigitalImageProcessing.Filters.NoiseReduction;

namespace DIPUI.ViewModels
{
    sealed class MedianFilter : Filter
    {
        public MedianFilter() : base("Median Filter", "Window Size")
        {
            FilterFunction = (img =>
                                  {
                                      var filter = new HistogramMedianFilter(Parameter);
                                      return filter.Filter(img);
                                  });
        }
    }
}
