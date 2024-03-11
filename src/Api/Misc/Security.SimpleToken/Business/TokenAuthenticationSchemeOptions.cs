using Microsoft.AspNetCore.Authentication;

namespace Rhyous.EntityAnywhere.Security
{
    /// <summary>A Simple Token Authentication scheme option for WebApi.</summary>
    public class TokenAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        /// <summary>The name of the scheme.</summary>
        public const string Name = "SimpleToken";
    }
}