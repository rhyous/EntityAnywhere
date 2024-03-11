using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services
{
    public interface ICredentialsValidatorLoader : IRuntimePluginLoader<ICredentialsValidatorAsync>, ICredentialsValidatorAsync
    {        
    }
}