using Newtonsoft.Json.Serialization;
using System;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public interface ISerializer
    {
        byte[] Json(object obj, IContractResolver resolver = null);
        object Deserialize(byte[] rawBody, Type type);
    }
}