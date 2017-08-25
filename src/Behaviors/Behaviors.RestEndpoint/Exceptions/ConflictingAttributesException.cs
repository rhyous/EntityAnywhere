using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Rhyous.WebFramework.Behaviors
{
    public class ConflictingAttributesException : Exception
    {
        public ConflictingAttributesException(List<Type> attributesTypes) : base($"Conflicting attributes.Only one of the following attributes can be applied: { string.Join(" ", attributesTypes.Select(t => t.Name))}")
        {
        }

        public ConflictingAttributesException(string message) : base(message)
        {
        }

        public ConflictingAttributesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConflictingAttributesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
