using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters
{
    class BitwiseAndFilter : FilterBase
    {
        private byte mask;

        public BitwiseAndFilter(byte mask) { this.mask = mask; }

        public override PGM.PgmImage Filter(PGM.PgmImage input)
        {
            var output = new PgmImage() { Header = input.Header, Data = new byte[input.Data.Length]};

            for (var i = 0; i < input.Data.Length; i++)
                output.Data[i] = (byte)(input.Data[i] & mask);

            return output;
        }

        public override string ToString()
        {
            return string.Format("Bitwise {0:X}", mask); 
        }
    }
}
