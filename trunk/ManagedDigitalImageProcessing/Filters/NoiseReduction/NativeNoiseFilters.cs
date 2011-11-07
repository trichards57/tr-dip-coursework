using System.Runtime.InteropServices;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    public static class NativeNoiseFilters
    {
        public static PgmImage BitwiseAndFilter(PgmImage input, byte mask)
        {
            var output = new PgmImage(input.Header);

            BitwiseAndFilter(input.Data.Length, input.Data, output.Data, mask);

            return output;
        }

        public static PgmImage MedianFilter(PgmImage input, int windowSize)
        {
            var output = new PgmImage(input.Header);

            MedianFilter(input.Data.Length, input.Data, output.Data, input.Header.Width, input.Header.Height, windowSize);

            return output;
        }

        public static PgmImage HistogramMedianFilter(PgmImage input, int windowSize)
        {
            var output = new PgmImage(input.Header);

            HistogramMedianFilter(input.Data.Length, input.Data, output.Data, input.Header.Width, input.Header.Height, windowSize);

            return output;
        }

        public static PgmImage AdaptiveHistogramMedianFilter(PgmImage input, int windowSize, double centreWeight, double scalingConstant)
        {
            var output = new PgmImage(input.Header);

            AdaptiveHistogramMedianFilter(input.Data.Length, input.Data, output.Data, input.Header.Width, input.Header.Height, windowSize, centreWeight, scalingConstant);

            return output;
        }

        [DllImport("NativeDigitalImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void BitwiseAndFilter(int dataLength, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] data, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] dataOut, byte mask);

        [DllImport("NativeDigitalImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void MedianFilter(int dataLength,
                                               [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] data,
                                               [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] dataOut,
                                               int picWidth, int picHeight, int windowSize);

        [DllImport("NativeDigitalImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void HistogramMedianFilter(int dataLength,
                                               [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] data,
                                               [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] dataOut,
                                               int picWidth, int picHeight, int windowSize);

        [DllImport("NativeDigitalImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void AdaptiveHistogramMedianFilter(int dataLength,
                                               [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] data,
                                               [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] dataOut,
                                               int picWidth, int picHeight, int windowSize, double centreWeight, double scalingConstant);
    }
}
