using System;
using System.Linq;
using System.Threading.Tasks;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    public sealed class SobelOperator : FilterBase
    {
        public override PgmImage Filter(PgmImage input)
        {
            var output = new PgmImage { Header = input.Header, Data = new byte[input.Header.Width * input.Header.Height] };

            var outputTemp = FilterSplit(input);

            Parallel.For(0, input.Data.Length, i =>
                                               output.Data[i] =
                                               (byte)
                                               Math.Sqrt(outputTemp.XData[i] * outputTemp.XData[i] +
                                                         outputTemp.XData[i] * outputTemp.XData[i]));

            return output;
        }

        public FilterResult FilterSplit(PgmImage input)
        {
            var output = new FilterResult { Header = input.Header };

            var template1 = new double[] { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
            var template2 = new double[] { 1, 2, 1, 0, 0, 0, -1, -2, -1 };

            var intData = Array.ConvertAll(input.Data, t => (int)t);

            var data = Convolve(template1, intData, new System.Drawing.Size(3, 3), new System.Drawing.Size(input.Header.Width, input.Header.Height));
            var absData = new int[data.Length];

            var data2 = data;
            Parallel.For(0, data2.Length, i => absData[i] = Math.Abs(data2[i]));

            checked
            {
                var max = absData.AsParallel().Max();
                var data1 = new byte[absData.Length];
                Parallel.For(0, absData.Length, i =>
                                                 {
                                                     data1[i] = (byte)(absData[i] * 255 / max);
                                                 });
                output.XData = data1;
            }

            data = Convolve(template2, intData, new System.Drawing.Size(3, 3), new System.Drawing.Size(input.Header.Width, input.Header.Height));
            Parallel.For(0, data.Length, i => absData[i] = Math.Abs(data[i]));
            checked
            {
                var max = absData.AsParallel().Max();
                var data1 = new byte[absData.Length];
                Parallel.For(0, absData.Length, i =>
                                                 {
                                                     data1[i] = (byte)(absData[i] * 255 / max);
                                                 });
                output.YData = data1;
            }

            return output;
        }
    }
}
