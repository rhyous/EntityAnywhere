using System;
using System.Diagnostics.CodeAnalysis;

namespace Rhyous.EntityAnywhere.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class ConfigurationException : Exception
    {
        public ConfigurationException(string message) : base(message)
        {
        }
    }
}