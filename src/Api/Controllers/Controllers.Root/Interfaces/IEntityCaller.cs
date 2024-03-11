using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IEntityCaller
    {
        Task<IEnumerable<T>> CallAll<T>(string rootUrl, string clientMethod)
            where T : class, IName;

        Task<T> CallOne<T>(string rootUrl, string clientMethod, Type entityType)
            where T : class, IName;
    }
}