using System;
using System.Web;

namespace Rhyous.WebFramework.WebServices
{
    public class Global : HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            var entityLoader = new EntityLoader();
            foreach (var item in entityLoader.Plugins)
            {
                EntityEndPointBuilder.BuildEntityRestService(item);
            }
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