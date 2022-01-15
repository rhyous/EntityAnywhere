using System;

namespace Rhyous.EntityAnywhere.Interfaces.Common.Tests
{
    public class FakeException : Exception
    {
        public FakeException()
        {
        }

        public FakeException(string message) : base(message)
        {
        }
    }
}
