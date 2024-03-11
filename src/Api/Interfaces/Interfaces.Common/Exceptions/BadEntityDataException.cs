using System;
using System.Runtime.Serialization;

namespace Rhyous.EntityAnywhere.Exceptions
{
    public class BadEntityDataException : Exception
    {
        public BadEntityDataException()
        {
        }

        public BadEntityDataException(string message) : base(message)
        {
        }

        public BadEntityDataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BadEntityDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
