using System;
using System.Threading.Tasks;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    class RobertsCrossOperator : FilterBase
    {
        public override PgmImage Filter(PgmImage input)
        {
            var output = new PgmImage { Header = input.Header, Data = new byte[input.Data.Length] };

            var template1 = new[] { 1, 0, 0, -1 };
            var template2 = new[] { 0, 1, -1, 0 };

            var val1 = Convolve(template1, input.Data, new System.Drawing.Size(2, 2), new System.Drawing.Size(input.Header.Width, input.Header.Height));
            var val2 = Convolve(template2, input.Data, new System.Drawing.Size(2, 2), new System.Drawing.Size(input.Header.Width, input.Header.Height));

            Parallel.For(0, val1.Length, i => output.Data[i] = Math.Max(val1[i], val2[i]));

            return output;
        }
    }
}
