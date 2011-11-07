using System.Runtime.InteropServices;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
{
    public static class NativeMorphologicalOperators
    {
        #region Convenience Functions

        public static PgmImage Erode(PgmImage input, int functionSize = 7, bool square = false)
        {
            var output = new PgmImage(input.Header);

            Erode(input.Data.Length, input.Data, output.Data, input.Header.Width, input.Header.Height, functionSize,
                  square);

            return output;
        }

        public static PgmImage Dilate(PgmImage input, int functionSize = 7, bool square = false)
        {
            var output = new PgmImage(input.Header);

            Dilate(input.Data.Length, input.Data, output.Data, input.Header.Width, input.Header.Height, functionSize,
                  square);

            return output;
        }

        public static PgmImage Close(PgmImage input, int functionSize = 7, bool square = false)
        {
            var output = new PgmImage(input.Header);

            Close(input.Data.Length, input.Data, output.Data, input.Header.Width, input.Header.Height, functionSize,
                  square);

            return output;
        }

        public static PgmImage Open(PgmImage input, int functionSize = 7, bool square = false)
        {
            var output = new PgmImage(input.Header);

            Open(input.Data.Length, input.Data, output.Data, input.Header.Width, input.Header.Height, functionSize,
                  square);

            return output;
        }

        public static PgmImage Gradient(PgmImage input, int functionSize = 7, bool square = false)
        {
            var output = new PgmImage(input.Header);

            Gradient(input.Data.Length, input.Data, output.Data, input.Header.Width, input.Header.Height, functionSize,
                  square);

            return output;
        }

        #endregion

        #region Function Imports
        [DllImport("NativeDigitalImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Erode(int dataLength,
                                         [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] data,
                                         [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] dataOut,
                                         int picWidth,
                                         int picHeight, int orderingFunctionSize, bool square);

        [DllImport("NativeDigitalImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Dilate(int dataLength,
                                         [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] data,
                                         [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] dataOut,
                                         int picWidth,
                                         int picHeight, int orderingFunctionSize, bool square);

        [DllImport("NativeDigitalImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Close(int dataLength,
                                         [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] data,
                                         [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] dataOut,
                                         int picWidth,
                                         int picHeight, int orderingFunctionSize, bool square);

        [DllImport("NativeDigitalImageProcessing.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Open(int dataLength,
                                         [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] data,
                                         [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] dataOut,
                                         int picWidth,
                                         int picHeight, int orderingFunctionSize, bool square);

        [DllImport("NativeDigitalImageProcessing.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "MorphologicalGradient")]
        private static extern void Gradient(int dataLength,
                                         [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] data,
                                         [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] byte[] dataOut,
                                         int picWidth,
                                         int picHeight, int orderingFunctionSize, bool square);
        #endregion
    }
}
