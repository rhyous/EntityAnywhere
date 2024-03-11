using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Rhyous.EntityAnywhere.Services
{
    [ExcludeFromCodeCoverage]
    public class DuplicateUsernameException : Exception
    {
        public DuplicateUsernameException()
        {
        }

        public DuplicateUsernameException(string message) : base(message)
        {
        }

        public DuplicateUsernameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicateUsernameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
