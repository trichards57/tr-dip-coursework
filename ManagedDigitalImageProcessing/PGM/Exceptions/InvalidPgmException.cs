using System;

namespace ManagedDigitalImageProcessing.PGM.Exceptions
{
    /// <summary>
    /// Exception thrown when the %PGM file is not valid.
    /// </summary>
    public sealed class InvalidPgmException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPgmException"/> class.
        /// </summary>
        public InvalidPgmException() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPgmException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidPgmException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPgmException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public InvalidPgmException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPgmException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected InvalidPgmException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
