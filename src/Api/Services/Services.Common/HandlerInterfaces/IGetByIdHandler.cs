﻿using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IGetByIdHandler<TEntity, TInterface, TId>
           where TEntity : class, TInterface, new()
           where TInterface : IBaseEntity<TId>
           where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        TInterface Get(TId Id);
    }
}