using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ManagedDigitalImageProcessing.Filters
{
    class NativeFilter
    {
        [DllImport("NativeDigitalImageProcessing.dll")]
        public static extern void BitwiseAndFilter([MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)]byte[] data, int dataLength, byte mask);
    }
}
