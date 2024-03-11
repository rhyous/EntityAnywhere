using Newtonsoft.Json;
using System;
using System.Net;

namespace Rhyous.EntityAnywhere.Models
{
    [Serializable]
    public class ServiceErrorResponseForwarderException : Exception
    {
        public ServiceErrorResponseForwarderException(string json, HttpStatusCode code)
        : base(json)
        {
            Response = (json.StartsWith("[") || json.StartsWith("{"))
                ? JsonConvert.DeserializeObject<ServiceErrorResponse>(json)
                : Response = new ServiceErrorResponse { Message = json, HResult = (int)code };
            Code = code;
        }

        public ServiceErrorResponseForwarderException(ServiceErrorResponse response, HttpStatusCode code)
            : base(JsonConvert.SerializeObject(response))
        {
            Response = response;
            Code = code;
        }

        public ServiceErrorResponse Response { get; }
        public HttpStatusCode Code { get; }
    }
}