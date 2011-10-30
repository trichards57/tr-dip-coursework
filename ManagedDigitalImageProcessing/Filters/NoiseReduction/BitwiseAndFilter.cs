using System.Threading.Tasks;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    public sealed class BitwiseAndFilter : FilterBase
    {
        private readonly byte _mask;

        public BitwiseAndFilter(byte mask) { _mask = mask; }

        public PgmImage Filter(PgmImage input)
        {
            var output = new PgmImage { Header = input.Header, Data = new byte[input.Data.Length] };

            Parallel.For(0, input.Data.Length, i => output.Data[i] = (byte)(input.Data[i] & _mask));

            return output;
        }

        public override string ToString()
        {
            return string.Format("Bitwise {0:X}", _mask);
        }
    }
}
