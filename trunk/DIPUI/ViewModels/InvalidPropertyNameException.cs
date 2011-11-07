using System;

namespace DIPUI.ViewModels
{
    public class InvalidPropertyException : Exception
    {
        public InvalidPropertyException()
        {
        }

        public InvalidPropertyException(string message) : base(message)
        {
        }

        public InvalidPropertyException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidPropertyException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
