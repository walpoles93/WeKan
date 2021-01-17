using System;
using System.Runtime.Serialization;

namespace WeKan.Application.Common.Exceptions
{
    public class UnauthorisedApplicationException : ApplicationException
    {
        public UnauthorisedApplicationException()
        {
        }

        public UnauthorisedApplicationException(string message) : base(message)
        {
        }

        public UnauthorisedApplicationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnauthorisedApplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
