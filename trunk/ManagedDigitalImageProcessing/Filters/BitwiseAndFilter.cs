using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters
{
    public sealed class BitwiseAndFilter : FilterBase
    {
        private readonly byte _mask;

        public BitwiseAndFilter(byte mask) { _mask = mask; }

        public override PgmImage Filter(PgmImage input)
        {
            var output = new PgmImage { Header = input.Header, Data = new byte[input.Data.Length]};

            for (var i = 0; i < input.Data.Length; i++)
                output.Data[i] = (byte)(input.Data[i] & _mask);

            return output;
        }

        public override string ToString()
        {
            return string.Format("Bitwise {0:X}", _mask); 
        }
    }
}
