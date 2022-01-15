using System;

namespace Rhyous.EntityAnywhere.Services
{
    public class DuplicateUsernameException : Exception
    {
        public DuplicateUsernameException(string message) : base(message)
        {
        }
    }
}
