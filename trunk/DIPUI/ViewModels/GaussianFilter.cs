using ManagedDigitalImageProcessing.Filters.NoiseReduction;

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
