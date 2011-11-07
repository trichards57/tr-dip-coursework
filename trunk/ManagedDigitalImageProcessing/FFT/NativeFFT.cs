using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ManagedDigitalImageProcessing.FFT
{
    public static class NativeFFT
    {
        [DllImport("NativeDigitalImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DitFFT(int dataCount,
                                          [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] double[] realIn,
                                          [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] double[] imagIn,
                                          [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] double[] realOut,
                                          [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] double[] imagOut);

        public static ComplexNumber[] DitFFT(ComplexNumber[] input)
        {
            var realIn = input.Select(n => n.Real).ToArray();
            var imagIn = input.Select(n => n.Imaginary).ToArray();

            var realOut = new double[realIn.Length];
            var imagOut = new double[realIn.Length];

            DitFFT(realIn.Length, realIn, imagIn, realOut, imagOut);

            return realOut.Zip(imagOut, (r, i) => new ComplexNumber(r, i)).ToArray();
        }
    }
}
