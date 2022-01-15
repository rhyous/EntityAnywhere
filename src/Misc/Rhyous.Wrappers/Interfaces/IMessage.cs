using System;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Xml;

namespace Rhyous.Wrappers
{
    public interface IMessage
    {
        Message Message { get; }
        MessageState State { get; }
        MessageProperties Properties { get; }
        MessageHeaders Headers { get; }
        bool IsFault { get; }
        bool IsEmpty { get; }
        MessageVersion Version { get; }
        IAsyncResult BeginWriteBodyContents(XmlDictionaryWriter writer, AsyncCallback callback, object state);
        IAsyncResult BeginWriteMessage(XmlDictionaryWriter writer, AsyncCallback callback, object state);
        void Close();
        MessageBuffer CreateBufferedCopy(int maxBufferSize);
        void EndWriteBodyContents(IAsyncResult result);
        void EndWriteMessage(IAsyncResult result);
        T GetBody<T>();
        T GetBody<T>(XmlObjectSerializer serializer);
        string GetBodyAttribute(string localName, string ns);
        XmlDictionaryReader GetReaderAtBodyContents();
        string ToString();
        void WriteBody(XmlWriter writer);
        void WriteBody(XmlDictionaryWriter writer);
        void WriteBodyContents(XmlDictionaryWriter writer);
        void WriteMessage(XmlDictionaryWriter writer);
        void WriteMessage(XmlWriter writer);
        void WriteStartBody(XmlWriter writer);
        void WriteStartBody(XmlDictionaryWriter writer);
        void WriteStartEnvelope(XmlDictionaryWriter writer);
    }
}