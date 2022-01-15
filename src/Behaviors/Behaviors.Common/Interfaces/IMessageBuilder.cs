using System.ServiceModel.Channels;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public interface IMessageBuilder
    {
        Message Build(MessageVersion messageVersion, string action, RawBodyWriter body, HttpResponseMessageProperty httpResponseMessageProperty = null);
    }
}