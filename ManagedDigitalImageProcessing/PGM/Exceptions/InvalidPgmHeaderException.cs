using System;

namespace ManagedDigitalImageProcessing.PGM.Exceptions
{
    [Serializable]
    public class InvalidPgmHeaderException : Exception
    {
        public InvalidPgmHeaderException() { }
        public InvalidPgmHeaderException(string message) : base(message) { }
        public InvalidPgmHeaderException(string message, System.Exception inner) : base(message, inner) { }
        protected InvalidPgmHeaderException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}