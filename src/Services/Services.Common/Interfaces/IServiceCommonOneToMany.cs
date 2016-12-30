using Rhyous.WebFramework.Interfaces;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Services
{
    public interface IServiceCommonOneToMany<T, Tinterface, Tid, TidRelated> : IServiceCommon<T, Tinterface, Tid>
        where T : class, Tinterface
        where Tinterface : IId<Tid>
    {
        string RelatedEntity { get; }
        string IdSuffix { get; }
        List<Tinterface> GetByRelatedEntityId(TidRelated id);
    }
}