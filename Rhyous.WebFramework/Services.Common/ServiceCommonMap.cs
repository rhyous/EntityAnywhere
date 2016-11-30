using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Services
{
    public class ServiceCommonMap<T, Tinterface> : ServiceCommon<T, Tinterface>, IServiceCommonMap<Tinterface> where T : class
    {
        public virtual string PrimaryEntity { get; }
        public virtual string SecondaryEntity { get; }

        public virtual List<Tinterface> GetByPropertyId(int id, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(PrimaryEntity) || string.IsNullOrWhiteSpace(SecondaryEntity))
                throw new InvalidOperationException("Both PrimaryEntity and SecondaryEntity must be assigned a value before this method is called.");
            if (propertyName != PrimaryEntity && propertyName != SecondaryEntity)
                throw new ArgumentException($"Valid property names are {PrimaryEntity} or {SecondaryEntity}", "propertyName");
            return Repo.GetByExpression(propertyName.ToLambda<T, int>(id)).ToList();
        }
    }
}