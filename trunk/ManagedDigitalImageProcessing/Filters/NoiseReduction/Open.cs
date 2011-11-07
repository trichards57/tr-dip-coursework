//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using ManagedDigitalImageProcessing.PGM;

//namespace ManagedDigitalImageProcessing.Filters.NoiseReduction
//{
//    class Open
//    {
//        private int _erodeSize;
//        private int _dilateSize;

//        public Open(int erodeSize, int dilateSize)
//        {
//            _erodeSize = erodeSize;
//            _dilateSize = dilateSize;
//        }

//        public Open(int filterSize) : this(filterSize, filterSize) { }

//        public PgmImage Filter(PgmImage image)
//        {
//            var erFilter = new Erode(_erodeSize);
//            var diFilter = new Dilate(_dilateSize);

//            return diFilter.Filter(erFilter.Filter(image));
//        }
//    }
//}
