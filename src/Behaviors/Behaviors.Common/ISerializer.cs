using Newtonsoft.Json.Serialization;

namespace Rhyous.WebFramework.Behaviors
{
    public interface ISerializer
    {
        byte[] Json(object obj, IContractResolver resolver = null);
    }
}