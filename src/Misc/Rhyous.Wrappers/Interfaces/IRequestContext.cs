using System;
using System.ServiceModel.Channels;

namespace Rhyous.Wrappers
{
    public interface IRequestContext
    {
        RequestContext Context { get; }
        IMessage RequestMessage { get; }

        void Abort();
        IAsyncResult BeginReply(Message message, AsyncCallback callback, object state);
        IAsyncResult BeginReply(Message message, TimeSpan timeout, AsyncCallback callback, object state);
        void Close();
        void Close(TimeSpan timeout);
        void EndReply(IAsyncResult result);
        void Reply(Message message);
        void Reply(Message message, TimeSpan timeout);
        void Dispose(bool disposing);
    }
}