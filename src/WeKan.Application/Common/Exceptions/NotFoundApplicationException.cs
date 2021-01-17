using System;
using System.Runtime.Serialization;

namespace WeKan.Application.Common.Exceptions
{
    public class NotFoundApplicationException : ApplicationException
    {
        public NotFoundApplicationException()
        {
        }

        public NotFoundApplicationException(string message) : base(message)
        {
        }

        public NotFoundApplicationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotFoundApplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
