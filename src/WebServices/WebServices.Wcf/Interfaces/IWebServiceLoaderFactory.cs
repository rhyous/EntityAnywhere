using System;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IWebServiceLoaderFactory
    {
        dynamic Create(Type t);
    }
}