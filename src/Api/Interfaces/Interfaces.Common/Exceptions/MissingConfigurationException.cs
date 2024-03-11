using System;
using System.Diagnostics.CodeAnalysis;

namespace Rhyous.EntityAnywhere.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class MissingConfigurationException : Exception
    {
        public MissingConfigurationException(string message) : base(message)
        {
        }
    }
}