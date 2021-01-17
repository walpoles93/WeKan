using System;
using System.Runtime.Serialization;

namespace WeKan.Application.Common.Exceptions
{
    public class InvalidOperationApplicationException : ApplicationException
    {
        public InvalidOperationApplicationException()
        {
        }

        public InvalidOperationApplicationException(string message) : base(message)
        {
        }

        public InvalidOperationApplicationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidOperationApplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
