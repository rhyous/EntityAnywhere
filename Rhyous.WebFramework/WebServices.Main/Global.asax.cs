using System;
using System.Web;
using System.Web.Routing;
using System.ServiceModel.Activation;

namespace Rhyous.WebFramework.WebServices
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //RouteTable.Routes.Add(new ServiceRoute("UserService", new WebServiceHostFactory(), typeof(UserWebService)));
            //RouteTable.Routes.Add(new ServiceRoute("UserTypeService", new WebServiceHostFactory(), typeof(UserTypeWebService)));
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