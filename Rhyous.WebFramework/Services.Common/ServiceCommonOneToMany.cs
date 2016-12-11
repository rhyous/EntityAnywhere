using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Services
{
    public class ServiceCommonOneToMany<T, Tinterface, Tid, TidRelated> : ServiceCommon<T, Tinterface, Tid>, IServiceCommonOneToMany<T, Tinterface, Tid, TidRelated> 
        where T : class, Tinterface
    {
        public virtual string RelatedEntity { get; }
        public virtual string IdSuffix => "Id";

        public virtual List<Tinterface> GetByRelatedEntityId(TidRelated id)
        {
            if (string.IsNullOrWhiteSpace(RelatedEntity))
                throw new InvalidOperationException("The RelatedEntity must be assigned a value before this method is called.");
            var relatedEntityColumnName = RelatedEntity + IdSuffix;
            return Repo.GetByExpression(relatedEntityColumnName.ToLambda<T, TidRelated>(id)).ToList();
        }
    }
}