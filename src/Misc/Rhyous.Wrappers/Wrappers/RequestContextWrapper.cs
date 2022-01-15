using System;
using System.ServiceModel.Channels;
namespace Rhyous.Wrappers
{
    public class RequestContextWrapper : IRequestContext
    {
        public RequestContextWrapper(RequestContext context)
        {
            Context = context;
            RequestMessage = new MessageWrapper(context.RequestMessage);
        }
        public RequestContext Context { get; }
        public IMessage RequestMessage { get; }
        public void Abort()
                => Context.Abort();
        public IAsyncResult BeginReply(Message message, AsyncCallback callback, object state)
                => Context.BeginReply(message, callback, state);
        public IAsyncResult BeginReply(Message message, TimeSpan timeout, AsyncCallback callback, object state)
                => Context.BeginReply(message, timeout, callback, state);
        public void Close()
                => Context.Close();
        public void Close(TimeSpan timeout)
                => Context.Close(timeout);
        public void EndReply(IAsyncResult result)
                => Context.EndReply(result);
        public void Reply(Message message)
                => Context.Reply(message);
        public void Reply(Message message, TimeSpan timeout)
                => Context.Reply(message, timeout);
        public void Dispose(bool disposing)
                    => Context.GetType()
                      .GetMethod(nameof(Dispose))
                      .Invoke(Context, new object[] { disposing });
    }
}
