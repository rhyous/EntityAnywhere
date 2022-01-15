using System;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Xml;
namespace Rhyous.Wrappers
{
    public class MessageWrapper : IMessage
    {
        public MessageWrapper(Message message)
                => Message = message;
        public Message Message { get; }
        public MessageState State => Message.State;
        public MessageProperties Properties => Message.Properties;
        public MessageHeaders Headers => Message.Headers;
        public bool IsFault => Message.IsFault;
        public bool IsEmpty => Message.IsEmpty;
        public MessageVersion Version => Message.Version;
        public IAsyncResult BeginWriteBodyContents(XmlDictionaryWriter writer, AsyncCallback callback, object state) => Message.BeginWriteBodyContents(writer, callback, state);
        public IAsyncResult BeginWriteMessage(XmlDictionaryWriter writer, AsyncCallback callback, object state) => Message.BeginWriteMessage(writer, callback, state);
        public void Close() => Message.Close();
        public MessageBuffer CreateBufferedCopy(int maxBufferSize) => Message.CreateBufferedCopy(maxBufferSize);
        public void EndWriteBodyContents(IAsyncResult result) => Message.EndWriteBodyContents(result);
        public void EndWriteMessage(IAsyncResult result)
                => Message.EndWriteMessage(result);
        public T GetBody<T>() => Message.GetBody<T>();
        public T GetBody<T>(XmlObjectSerializer serializer) => Message.GetBody<T>(serializer);
        public string GetBodyAttribute(string localName, string ns) => Message.GetBodyAttribute(localName, ns);
        public XmlDictionaryReader GetReaderAtBodyContents() => Message.GetReaderAtBodyContents();
        public override string ToString() => Message.ToString();
        public void WriteBody(XmlWriter writer)
                => Message.WriteBody(writer);
        public void WriteBody(XmlDictionaryWriter writer)
                => Message.WriteBody(writer);
        public void WriteBodyContents(XmlDictionaryWriter writer)
                => Message.WriteBodyContents(writer);
        public void WriteMessage(XmlDictionaryWriter writer)
                => Message.WriteMessage(writer);
        public void WriteMessage(XmlWriter writer)
                => Message.WriteMessage(writer);
        public void WriteStartBody(XmlWriter writer)
                => Message.WriteStartBody(writer);
        public void WriteStartBody(XmlDictionaryWriter writer)
                => Message.WriteStartBody(writer);
        public void WriteStartEnvelope(XmlDictionaryWriter writer)
                => Message.WriteStartEnvelope(writer);
    }
}