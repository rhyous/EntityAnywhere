using Rhyous.StringLibrary.Pluralization;
using Rhyous.WebFramework.Clients2;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services.Security;
using System;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Authenticators
{
    /// <summary>
    /// This is the primary login method as part of Entity Anywhere framework. This logs in using the User entity.
    /// </summary>
    public class ActivationCredentialCredentialsValidator : ICredentialsValidatorAsync
    {
        private readonly IAdminEntityClientAsync<ActivationCredential, long> _ActivationClient;
        private readonly IPasswordSecurity _PasswordSecurity;
        private readonly IClaimsBuilder _ClaimsBuilder;
        private readonly IJWTToken _JwtToken;
        private readonly ILogger _Logger;

        public ActivationCredentialCredentialsValidator(
                                        IAdminEntityClientAsync<ActivationCredential, long> activationCredentialClient,
                                        IPasswordSecurity passwordSecurity,
                                        IClaimsBuilder claimsBuilder,
                                        IJWTToken jwtToken,
                                        ILogger logger)
        {
            _ActivationClient = activationCredentialClient;
            _PasswordSecurity = passwordSecurity;
            _ClaimsBuilder = claimsBuilder;
            _JwtToken = jwtToken;
            _Logger = logger;
        }

        public string Name => nameof(ActivationCredential).Pluralize();

        /// <inheritdoc />
        public async Task<CredentialsValidatorResponse> IsValidAsync(ICredentials creds)
        {
            if (creds == null || string.IsNullOrWhiteSpace(creds.User) || string.IsNullOrWhiteSpace(creds.Password))
                throw new ArgumentException(nameof(creds));
            var response = new CredentialsValidatorResponse { AuthenticationPlugin = Name };
            var odataActivationCredential = await _ActivationClient.GetByAlternateKeyAsync(creds.User);
            var activationCredential = odataActivationCredential?.Object;

            // Make sure activation credential is found by user and enaled
            if (activationCredential == null)
            {
                response.Message = $"This {Name} user '{creds.User}' was not found.";
                return response;
            }
            if (!activationCredential.Enabled)
            {
                response.Message = $"This {Name} user '{creds.User}' is disabled.";
                return response;
            }

            // Check the passwords match
            if (!_PasswordSecurity.Compare(creds.Password, activationCredential.Password))
            {   // Try by trimming the provided password, just in case they copy and pasted with a space, which is a top support issue
                if (!_PasswordSecurity.Compare(creds.Password.Trim(), activationCredential.Password))
                {
                    response.Message = $"The password provided for {Name} user '{creds.User}' is incorrect.";
                    return response;
                }
            }

            response.Token = new Token
            {
                ClaimDomains = await _ClaimsBuilder.BuildAsync(activationCredential),
                CredentialEntityId = activationCredential.Id,
            };
            response.Token.Text = _JwtToken.GetTokenText(response.Token.ClaimDomains);
            response.Success = true;
            return response;
        }
    }
}