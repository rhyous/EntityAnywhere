using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.Odata;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IGetByPropertyValuePairsHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IExtensionEntity, IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        OdataObjectCollection<TEntity, TId> Handle(IEnumerable<PropertyValue> propertyValues);
    }
}