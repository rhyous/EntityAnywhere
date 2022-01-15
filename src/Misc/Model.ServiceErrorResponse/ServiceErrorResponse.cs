using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.EntityAnywhere.Models
{
    [DataContract]
    public class ServiceErrorResponse
    {
        public ServiceErrorResponse() { }

        public ServiceErrorResponse(Exception ex, bool acknowlegeable = false, string messageOverride = null)
        {
            Message = messageOverride ?? ex.Message;
            Source = ex.Source;
            HResult = ex.HResult;
            Data = ex.Data.ConvertDictionaryToPair();
            Type = ex.GetType().ToString();
            Acknowledgeable = acknowlegeable;
        }

        [DataMember]
        public string Message { get; set; }

        public int HResult { get; set; }

        [DataMember]
        public string Source { get; set; }

        [DataMember]
        public IEnumerable<KeyValuePair<string, ErrorResponseValues>> Data { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public bool Acknowledgeable { get; set; }        
    }
}