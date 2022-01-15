using System.ServiceModel.Channels;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public interface ICustomBindingProvider
    {
        Binding Get(string bindingName);
        Binding Get(string entity, string scheme);
    }
}
