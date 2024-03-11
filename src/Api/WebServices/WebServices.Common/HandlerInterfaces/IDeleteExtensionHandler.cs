using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IDeleteExtensionHandler<TEntity, TInterface, TId>
                where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        Task<Dictionary<long, bool>> HandleAsync(string id, string extensionEntity);

        Task<Dictionary<long, bool>> HandleAsync(string id, string extensionEntity, IEnumerable<string> extentionEntityIds);
    }
}
