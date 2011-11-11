using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.PGM;
using System.Threading.Tasks;

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    class Threshold
    {
        private byte _threshold;

        public Threshold(byte threshold)
        {
            _threshold = threshold;
        }

        public PgmImage Filter(PgmImage input)
        {
            var output = new PgmImage { Header = input.Header, Data = new byte[input.Data.Length] };

            Parallel.For(0, input.Data.Length, i =>
                {
                    if (input.Data[i] < _threshold)
                        output.Data[i] = 0;
                    else
                        output.Data[i] = input.Data[i];
                });

            return output;
        }
    }
}
