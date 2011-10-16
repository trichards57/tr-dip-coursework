using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters
{
    class FilterResult
    {
        public PgmHeader Header { get; set; }

        public byte[] XData { get; set; }
        public byte[] YData { get; set; }
    }
}
