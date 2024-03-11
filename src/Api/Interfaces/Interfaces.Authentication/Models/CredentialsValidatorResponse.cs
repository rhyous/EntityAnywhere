using System.Diagnostics.CodeAnalysis;

namespace Rhyous.EntityAnywhere.Interfaces
{
    [ExcludeFromCodeCoverage]
    public class CredentialsValidatorResponse
    {
        public bool Success { get; set; }
        public IToken Token { get; set; }
        public string AuthenticationPlugin { get; set; }
        public string Message { get; set; }
    }
}