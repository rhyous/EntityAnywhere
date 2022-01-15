using System;
using System.Runtime.Serialization;

namespace Rhyous.EntityAnywhere.AutomatedTests
{
    [Serializable]
    internal class CircularEntityDependecyException : Exception
    {
        public CircularEntityDependecyException()
        {
        }

        public CircularEntityDependecyException(string message) : base(message)
        {
        }

        public CircularEntityDependecyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CircularEntityDependecyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}