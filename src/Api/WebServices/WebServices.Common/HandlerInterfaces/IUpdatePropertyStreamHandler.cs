using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IUpdatePropertyStreamHandler<TEntity, TInterface, TId>
       where TEntity : class, TInterface, new()
       where TInterface : IBaseEntity<TId>
       where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        Task<bool> HandleAsync(string id, string property, Stream value);
    }
}