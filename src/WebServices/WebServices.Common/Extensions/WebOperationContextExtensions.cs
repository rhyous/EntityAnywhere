using System.ServiceModel.Web;

namespace Rhyous.WebFramework.WebServices.Extensions
{
    public static partial class WebOperationContextExtensions
    {
        public const string TokenName = "Token";

        public static string GetToken(this WebOperationContext webContext)
        {
            return GetHeader(webContext, TokenName);
        }
        private static string GetHeader(WebOperationContext webContext, string header)
        {
            if (webContext == null || string.IsNullOrWhiteSpace(webContext.IncomingRequest.Headers[header]))
                return null;
            return webContext.IncomingRequest.Headers[header];
        }
    }
}
