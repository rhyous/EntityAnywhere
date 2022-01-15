using System.ServiceModel.Channels;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public interface IMessageHandler
    {
        IMessageProperty GetHttpRequestMessageProperty(Message message, string name);
        WebContentFormat GetMessageContentFormat(Message message);
        string MessageToString(ref Message message);
        bool LogRequestMessage { get; }
        bool LogResponseMessage { get; }
    }
}