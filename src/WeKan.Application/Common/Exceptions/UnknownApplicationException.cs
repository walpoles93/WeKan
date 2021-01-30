using System;
using System.Runtime.Serialization;

namespace WeKan.Application.Common.Exceptions
{
    public class UnknownApplicationException : ApplicationException
    {
        public UnknownApplicationException()
        {
        }

        public UnknownApplicationException(string message) : base(message)
        {
        }

        public UnknownApplicationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnknownApplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
