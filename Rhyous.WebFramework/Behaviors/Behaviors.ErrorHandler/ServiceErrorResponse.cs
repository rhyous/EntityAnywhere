using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Rhyous.WebFramework.Behaviors
{
    [DataContract]
    public class ServiceErrorResponse
    {
        public ServiceErrorResponse(Exception ex, string messageOverride = null)
        {
            Message = messageOverride ?? ex.Message;
            Source = ex.Source;
            HResult = ex.HResult;
            Data = ex.Data;
        }
        [DataMember]
        public String Message { get; set; }

        public int HResult { get; set; }

        [DataMember]
        public String Source { get; set; }

        [DataMember]
        public IDictionary Data { get; set; }
    }
}
