using Rhyous.WebFramework.Interfaces;
using System;


namespace Rhyous.WebFramework.Services
{
    public class PluginCredentialsValidator : PluginLoaderBase<ICredentialsValidator>, ICredentialsValidator
    {
        public override string PluginSubFolder => "Authenticators";
        
        public bool IsValid(ICredentials creds, out IToken token)
        {
            foreach (var plugin in Plugins)
            {
                if (plugin.IsValid(creds, out token))
                {
                    return true;
                }
            }
            token = null;
            return false;
        }
    }
}