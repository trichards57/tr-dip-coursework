using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagedDigitalImageProcessing.Filters
{
    class NoOpFilter : FilterBase
    {
        public override PGM.PgmImage Filter(PGM.PgmImage input)
        {
            return input;
        }

        public override string ToString()
        {
            return "NoOp";
        }
    }
}
