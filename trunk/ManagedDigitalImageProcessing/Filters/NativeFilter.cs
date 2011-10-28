using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ManagedDigitalImageProcessing.Filters
{
    class NativeFilter
    {
        [DllImport("NativeDigitalImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BitwiseAndFilter(int dataLength, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] byte[] data, byte mask);
    }
}
