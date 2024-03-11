using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Security
{
    /// <summary>An interface describing a PluginHeaderValidator</summary>
    public interface IPluginHeaderValidator : IHeaderValidator, IRuntimePluginLoader<IHeaderValidator> { }
}