using System.Runtime.InteropServices;

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    public class NativeFilters
    {
        [DllImport("NativeDigitalImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BitwiseAndFilter(int dataLength, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] data, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] dataOut, byte mask);

        [DllImport("NativeDigitalImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MedianFilter(int dataLength,
                                               [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] data,
                                               [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] dataOut,
                                               int picWidth, int picHeight, int windowSize);

        [DllImport("NativeDigitalImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void HistogramMedianFilter(int dataLength,
                                               [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] data,
                                               [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] dataOut,
                                               int picWidth, int picHeight, int windowSize);

        [DllImport("NativeDigitalImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AdaptiveHistogramMedianFilter(int dataLength,
                                               [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] data,
                                               [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] dataOut,
                                               int picWidth, int picHeight, int windowSize, double centreWeight, double scalingConstant);
    }
}
