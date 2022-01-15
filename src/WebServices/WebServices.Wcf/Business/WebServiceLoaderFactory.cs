using Autofac;
using System;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class WebServiceLoaderFactory : IWebServiceLoaderFactory
    {
        private readonly IComponentContext _Context;

        public WebServiceLoaderFactory(IComponentContext context)
        {
            _Context = context;
        }

        public dynamic Create(Type t)
        {
            return _Context.Resolve(t);
        }
    }
}
