﻿using Rhyous.Odata;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IGetByIdHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        Task<OdataObject<TEntity, TId>> HandleAsync(string id);
        Task<OdataObject<TEntity, TId>> GetByTIdAsync(TId id);
    }
}
