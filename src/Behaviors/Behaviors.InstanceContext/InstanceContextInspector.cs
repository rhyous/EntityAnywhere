using Rhyous.WebFramework.Interfaces;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.Behaviors
{
    public class InstanceContextInspector : IDispatchMessageInspector
    {
        #region IDispatchMessageInspector interface
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            Add("OperationContext", OperationContext.Current);
            Add("WebOperationContext", WebOperationContext.Current);
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }
        #endregion

        internal static IDictionaryDefaultValueProvider<string, object> LockDictionary
        {
            get { return _LockDictionary ?? (_LockDictionary = new NullSafeDictionary<string, object>()); }
            set { _LockDictionary = value; }
        } private static IDictionaryDefaultValueProvider<string, object> _LockDictionary;
        
        internal void Add(string key, object o)
        {
            if (CallContext.LogicalGetData(key) == null)
            {
                lock (LockDictionary[key])
                {
                    if (CallContext.LogicalGetData(key) == null)
                        CallContext.LogicalSetData(key, o);
                }
            }
        }
    }
}