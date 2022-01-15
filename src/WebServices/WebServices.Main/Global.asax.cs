using System;
using System.Web;

namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// This class exists to do start EAF.
    /// </summary>
    public class Global : HttpApplication
    {
        /// <summary>
        /// This method runs when the web site/application pool starts and runs Starter.Start().
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs e)
        {
            Starter.Start();
        }

        protected void Session_Start(object sender, EventArgs e)
        {            
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
#if RELEASE
#else
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
#endif
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PATCH, PUT, DELETE");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Access-Control-Allow-Origin, Origin, X-Requested-With, Content-Type, Accept, x-ms-request-id, x-ms-request-root-id, Token, OrganizationId");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}