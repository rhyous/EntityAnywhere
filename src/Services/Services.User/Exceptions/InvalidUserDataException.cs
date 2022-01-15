using System;

namespace Rhyous.EntityAnywhere.Services
{
    public class InvalidUserDataException : Exception
    {
        public InvalidUserDataException(string message) : base(message)
        {
        }
    }
}
