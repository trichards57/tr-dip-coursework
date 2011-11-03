using ManagedDigitalImageProcessing.PGM;
using ManagedDigitalImageProcessing.Filters.NoiseReduction;

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    class MorphologicalGradient
    {
        private int _erodeSize;
        private int _dilateSize;

        public MorphologicalGradient(int erodeSize, int dilateSize)
        {
            _erodeSize = erodeSize;
            _dilateSize = dilateSize;
        }

        public MorphologicalGradient(int filterSize) : this(filterSize, filterSize) { }

        public PgmImage Filter(PgmImage image)
        {
            var diFilter = new Dilate(_dilateSize);
            var erFilter = new Erode(_erodeSize);
            var er = erFilter.Filter(image);
            var di = diFilter.Filter(image);

            var output = new PgmImage { Header = image.Header, Data = new byte[er.Data.Length] };

            var buffer = new int[er.Data.Length];
            var max = 0;

            for (var i = 0; i < er.Data.Length; i++)
            {
                var val = di.Data[i] - er.Data[i];

                if (val < 0)
                    val = 0;

                buffer[i] = val;
                if (val > max)
                    max = val;
            }
            for (var i = 0; i < er.Data.Length; i++)
            {
                var scale = 255.0 / max;

                output.Data[i] = (byte)(scale * buffer[i]);
            }

            return output;
        }
    }
}
