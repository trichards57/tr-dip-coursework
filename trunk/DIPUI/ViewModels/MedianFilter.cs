using ManagedDigitalImageProcessing.Filters.NoiseReduction;

namespace DIPUI.ViewModels
{
    sealed class MedianFilter : Filter
    {
        public MedianFilter() : base("Median Filter", "Window Size")
        {
            FilterFunction = (img => NativeNoiseFilters.HistogramMedianFilter(img, Parameter));
        }
    }
}
