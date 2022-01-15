using System;
using System.Runtime.Serialization;

namespace Rhyous.EntityAnywhere.AutomatedTests
{
    [Serializable]
    internal class PrerequisiteEntityMissingException : Exception
    {
        public PrerequisiteEntityMissingException()
        {
        }

        public PrerequisiteEntityMissingException(string message) : base(message)
        {
        }

        public PrerequisiteEntityMissingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PrerequisiteEntityMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}