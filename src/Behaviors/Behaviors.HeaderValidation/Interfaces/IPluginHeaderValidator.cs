using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public interface IPluginHeaderValidator : IHeaderValidator, IRuntimePluginLoader<IHeaderValidator> { }
}