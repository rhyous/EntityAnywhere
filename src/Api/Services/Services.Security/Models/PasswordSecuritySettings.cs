using Rhyous.Collections;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.Security
{
    /// <summary>
    /// Concrete implementation of <see cref="IPasswordSecuritySettings"/>. Immutable once created
    /// </summary>
    public class PasswordSecuritySettings : IPasswordSecuritySettings
    {
        private readonly IAppSettings _AppSettings;

        internal const string SystemSecurityKey = "SystemSecurityKey";
        internal const string SystemSecurityDerivationIterations = "SystemSecurityDerivationIterations";
        internal const int DefaultSystemSecurityDerivationIterations = 100;

        /// <summary>
        /// Construct a new <see cref="PasswordSecuritySettings"/> with the required dependencies
        /// </summary>
        /// <param name="cypherKey">The cypher key</param>
        /// <param name="keySizeInBits">The size of the key in bits</param>
        /// <param name="derivationIterations">The amount of iterations to encrpyt and decrpy the password</param>
        public PasswordSecuritySettings(IAppSettings appSettings)
        {
            _AppSettings = appSettings;
        }

        /// <inheritdoc/>
        public string CipherKey => _AppSettings.Collection.Get<string>(SystemSecurityKey, null)
                                ?? throw new MissingConfigurationException($"AppSettings must contain a setting for {SystemSecurityKey}");

        /// <inheritdoc/>
        public int DerivationIterations => _AppSettings.Collection.Get(SystemSecurityDerivationIterations, DefaultSystemSecurityDerivationIterations);
    }
}
