using System;
using System.Diagnostics.CodeAnalysis;

namespace Rhyous.EntityAnywhere.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class DuplicateKeyException : Exception
    {
        public DuplicateKeyException(string property, string message) 
            : base($"The property {property} must be unique.{Environment.NewLine}{message}")
        {
        }
    }
}