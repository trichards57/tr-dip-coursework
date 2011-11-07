//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using ManagedDigitalImageProcessing.PGM;

//namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
//{
//    class Close
//    {
//        private int _erodeSize;
//        private int _dilateSize;

//        public Close(int erodeSize, int dilateSize)
//        {
//            _erodeSize = erodeSize;
//            _dilateSize = dilateSize;
//        }

//        public Close(int filterSize) : this(filterSize, filterSize) { }

//        public PgmImage Filter(PgmImage image)
//        {
//            var erFilter = new Erode(_erodeSize);
//            var diFilter = new Dilate(_dilateSize);

//            return erFilter.Filter(diFilter.Filter(image));
//        }
//    }
//}
