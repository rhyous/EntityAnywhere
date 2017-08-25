using System;
using System.ServiceModel.Web;
using System.Web;

namespace Rhyous.WebFramework.WebServices
{
    public interface IHttpContextProvider
    {
        HttpContext CurrentHttpContext { get; }
        WebOperationContext CurrentWebOperationContext { get; }

        string WebHost { get; }

        Uri Uri { get; }
    }
}