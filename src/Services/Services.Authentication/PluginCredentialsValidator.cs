using Rhyous.WebFramework.Clients;
using Rhyous.WebFramework.Interfaces;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Services
{
    /// <summary>
    /// This loads the credential validator plugins.
    /// </summary>
    public class PluginCredentialsValidator : PluginLoaderBase<ICredentialsValidatorAsync>, ICredentialsValidator
    {
        public override string PluginSubFolder => "Authenticators";

        public bool IsValid(ICredentials creds, out IToken token)
        {
            token = TaskRunner.RunSynchonously(IsValidAsync, creds, WebOperationContext.Current);
            return token != null;
        }

        internal async Task<IToken> IsValidAsync(ICredentials creds, WebOperationContext context)
        {
            foreach (var plugin in Plugins)
            {
                var token = await plugin.IsValidAsync(creds, context);
                if (token != null)
                    return token;
            }
            return null;
        }
    }
}