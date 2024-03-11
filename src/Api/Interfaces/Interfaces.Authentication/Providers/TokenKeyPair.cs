using Rhyous.Collections;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.Wrappers;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class TokenKeyPair : ITokenKeyPair
    {
        private readonly IAppSettings _AppSettings;
        private readonly IFileIO _File;

        public TokenKeyPair(IAppSettings appSettings,
                            IFileIO file)
        {
            _AppSettings = appSettings;
            _File = file;
        }
        public string PrivateKey => _PrivateKey ?? (_PrivateKey = GetKey("Private"));
        private string _PrivateKey;

        public string PublicKey => _PublicKey ?? (_PublicKey = GetKey("Public"));
        private string _PublicKey;

        internal string GetKey(string publicOrPrivate)
        {
            var path = _AppSettings.Collection.Get($"JWT{publicOrPrivate}Key", "");

            if (string.IsNullOrWhiteSpace(path))
                throw new MissingConfigurationException($"JWT{publicOrPrivate}Key setting key is missing. Please set JWT{publicOrPrivate}Key value in web.config's app settings.");

            if (!_File.Exists(path))
                throw new ConfigurationException($"JWT{publicOrPrivate}Key file is missing. Expecting it at path: {path}");

            string privateKey = _File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(privateKey))
                throw new ConfigurationException($"JWT{publicOrPrivate}Key file appears to be empty at path: {path}");

            return privateKey;
        }
    }
}