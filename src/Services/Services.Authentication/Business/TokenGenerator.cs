using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    /// <summary>
    /// A class to generate an authentication token.
    /// </summary>
    public partial class TokenGenerator : ITokenBuilder<IUser>, ILogProperty
    {
        private readonly IClaimsBuilderAsync ClaimsBuilder;
        private readonly IJWTToken JWTToken;

        public TokenGenerator(
            IClaimsBuilderAsync claimsBuilder,
            IJWTToken jwtToken,
            ILogger logger)
        { 
            Logger = logger;
            ClaimsBuilder = claimsBuilder;
            JWTToken = jwtToken;
        }

        public ILogger Logger { get; set; }

        /// <inheritdoc />
        public virtual async Task<IToken> BuildAsync(ICredentials creds, IUser user)
        {
            var token = new Token { CredentialEntityId = user.Id, CredentialEntity = nameof(User) };
            user = await ClaimsBuilder.BuildAsync(user.Id, token);
            token.Text = JWTToken.GetTokenText(token.ClaimDomains);
            return token;
        }
    }
}