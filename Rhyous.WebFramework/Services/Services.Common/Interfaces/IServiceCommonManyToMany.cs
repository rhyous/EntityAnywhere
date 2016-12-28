using Rhyous.WebFramework.Interfaces;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Services
{
    public interface IServiceCommonManyToMany<T,Tinterface, Tid, TidPrimary, TidSecondary> : IServiceCommon<T, Tinterface, Tid>
        where T : class, Tinterface
        where Tinterface : IId<Tid>
    {
        string PrimaryEntity { get; }
        string SecondaryEntity { get; }
        string IdSuffix { get; }
        List<Tinterface> GetByRelatedEntityId(object id, string entity);
        List<Tinterface> GetByRelatedPrimaryEntityId(TidPrimary id, string entity);
        List<Tinterface> GetByRelatedSecondaryEntityId(TidSecondary id, string entity);
    }
}