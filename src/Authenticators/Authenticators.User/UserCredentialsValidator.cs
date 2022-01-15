using Rhyous.Collections;
using Rhyous.StringLibrary.Pluralization;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Authenticators
{
    /// <summary>
    /// This is the primary login method as part of Entity Anywhere framework. This logs in using the User entity.
    /// </summary>
    public class UserCredentialsValidator : ICredentialsValidatorAsync
    {
        internal const string AuthenticateExternallySetting = "ForceExternalUsersToAuthenticateExternally";
        private readonly ITokenBuilder<IUser> _TokenBuilder;
        private readonly IAdminEntityClientAsync<User, long> _UserClient;
        private readonly IAppSettings _AppSettings;

        public UserCredentialsValidator(ITokenBuilder<IUser> tokenGenerator,
                                        IAdminEntityClientAsync<User, long> userClient,
                                        IAppSettings appSettings)
        {
            _TokenBuilder = tokenGenerator;
            _UserClient = userClient;
            _AppSettings = appSettings;
        }

        public string Name => nameof(User).Pluralize();

        /// <summary>
        /// If this is true, external users cannot login using this plugin.
        /// If this is false, users can login either with this plugin or with another plugin.
        /// </summary>
        public bool ForceExternalUsersToAuthenticateExternally { get { return _AppSettings.Collection.Get(AuthenticateExternallySetting, true); } }

        /// <inheritdoc />
        public async Task<CredentialsValidatorResponse> IsValidAsync(ICredentials creds)
        {
            if (creds == null || string.IsNullOrWhiteSpace(creds.User) || string.IsNullOrWhiteSpace(creds.Password))
                throw new ArgumentException(nameof(creds));

            var response = new CredentialsValidatorResponse { AuthenticationPlugin = Name };
            var odataUser = await _UserClient.GetByAlternateKeyAsync(creds.User, "?$expand=UserRole");
            var user = odataUser?.Object;
            
            if (user == null)
            {
                response.Message = $"This user {creds.User} was not found.";
                return response;
            }
            if (!user.Enabled)
            {
                response.Message = $"This user {creds.User} is disabled.";
                return response;
            }
            if (user.ExternalAuth && ForceExternalUsersToAuthenticateExternally)
            {
                response.Message = $"This user {creds.User} can only authenticate with external authenticators.";
                return response;
            }

            var roles = odataUser.RelatedEntityCollection.FirstOrDefault(re => re.RelatedEntity == "UserRole");
            if (roles == null)
            {
                response.Message = "The user has not been configured with a user role.";
                return response;
            }
            bool result;
            if (user.IsHashed)
            {
                result = Hash.Compare(creds.Password, user.Salt, user.Password, Hash.DefaultHashType, Hash.DefaultEncoding);
                if (!result) // Try by trimming the provided password, just in case they copy and pasted with a space, which is a top support issue
                    result = Hash.Compare(creds.Password.Trim(), user.Salt, user.Password, Hash.DefaultHashType, Hash.DefaultEncoding);
            }
            else
            {
                result = creds.Password == user.Password;
                if (!result) // Try by trimming the provided password, just in case they copy and pasted with a space, which is a top support issue
                    result = creds.Password.Trim() == user.Password;
            }
            if (!result)
            {
                response.Message = $"The provided {Name} password is invalid.";
                return response;
            }
            response.Token = await _TokenBuilder.BuildAsync(creds, user);
            response.Success = true;
            return response;
        }
    }
}
