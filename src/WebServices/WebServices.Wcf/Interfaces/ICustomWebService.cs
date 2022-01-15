using System;

namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// This is used to identify a custom webservice. We could change to use an Attribute in the future, but PluginLoader works with types not attributes, so we used an interface.
    /// </summary>
    public interface ICustomWebService : IDisposable
    {
    }
}
