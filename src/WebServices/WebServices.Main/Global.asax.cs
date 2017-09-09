using System;
using System.Web;

namespace Rhyous.WebFramework.WebServices
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