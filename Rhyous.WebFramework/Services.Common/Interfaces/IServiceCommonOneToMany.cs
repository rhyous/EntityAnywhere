using System.Collections.Generic;

namespace Rhyous.WebFramework.Services
{
    public interface IServiceCommonOneToMany<T, Tinterface> : IServiceCommon<T, Tinterface>
        where T : class, Tinterface
    {
        string RelatedEntity { get; }
        string IdSuffix { get; }
        List<Tinterface> GetByRelatedEntityId(int id);
    }
}