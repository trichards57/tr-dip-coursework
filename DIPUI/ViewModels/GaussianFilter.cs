using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.Filters;

namespace DIPUI.ViewModels
{
    class GaussianFilter : Filter
    {
        public GaussianFilter() : base("Gaussian Filter", "Sigma")
        {
            FilterFunction = (img =>
                                  {
                                      var filter = new SeperatedGaussianFilter(Parameter);
                                      return filter.Filter(img);
                                  });
        }
    }
}
