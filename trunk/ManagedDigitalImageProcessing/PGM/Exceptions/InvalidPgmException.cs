using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagedDigitalImageProcessing.PGM.Exceptions
{
    class InvalidPgmException : Exception
    {
        public InvalidPgmException() { }
        public InvalidPgmException(string message) : base(message) { }
        public InvalidPgmException(string message, System.Exception inner) : base(message, inner) { }
        protected InvalidPgmException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
