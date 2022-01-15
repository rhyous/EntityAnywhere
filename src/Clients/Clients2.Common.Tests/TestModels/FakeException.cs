using System;

namespace Rhyous.EntityAnywhere.Clients2.Common.Tests.Tools
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
