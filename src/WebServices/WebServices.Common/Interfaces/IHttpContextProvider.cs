using System;
using System.ServiceModel.Web;
using System.Web;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// An interface for a class that tries to figure out the web service url.
    /// </summary>
    public interface IHttpContextProvider
    {
        /// <summary>
        /// The HttpContext.Current object, if it exists and can be accessed.
        /// </summary>
        HttpContext CurrentHttpContext { get; set; }

        /// <summary>
        /// The WebOperationContext.Current object, if it exists and can be accessed.
        /// </summary>
        WebOperationContext CurrentWebOperationContext { get; set; }

        /// <summary>
        /// The 
        /// </summary>
        string WebHost { get; }

        /// <summary>
        /// The url used if this object was created by a web call, otherwise, this value must be speficied.
        /// </summary>
        Uri Uri { get; }
    }
}