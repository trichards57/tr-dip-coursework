using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    public static class NativeMorphologicalOperators
    {
        [DllImport("NativeDigitalImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Erode(int dataLength,
                                         [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] data,
                                         [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] dataOut,
                                         int picWidth,
                                         int picHeight, int orderingFunctionSize, bool square);

        [DllImport("NativeDigitalImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Dilate(int dataLength,
                                         [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] data,
                                         [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] dataOut,
                                         int picWidth,
                                         int picHeight, int orderingFunctionSize, bool square);

        [DllImport("NativeDigitalImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Close(int dataLength,
                                         [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] data,
                                         [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] dataOut,
                                         int picWidth,
                                         int picHeight, int orderingFunctionSize, bool square);

        [DllImport("NativeDigitalImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Open(int dataLength,
                                         [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] data,
                                         [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] dataOut,
                                         int picWidth,
                                         int picHeight, int orderingFunctionSize, bool square);
    }
}
