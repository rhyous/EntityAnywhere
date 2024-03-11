using System.Net;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IHttpStatusCodeSetter
    {
        HttpStatusCode StatusCode { get; set; }
    }
}
