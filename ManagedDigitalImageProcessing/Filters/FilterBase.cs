using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters
{
    abstract class FilterBase
    {
        protected static int CalculateIndex(int x, int y, int width)
        {
            return x + y * width;
        }

        public abstract PgmImage Filter(PgmImage input);
    }
}
