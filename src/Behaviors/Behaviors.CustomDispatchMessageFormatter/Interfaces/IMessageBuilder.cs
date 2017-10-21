using System.ServiceModel.Channels;

namespace Rhyous.WebFramework.Behaviors
{
    public interface IMessageBuilder
    {
        Message Build(MessageVersion messageVersion, string action, RawBodyWriter body);
    }
}