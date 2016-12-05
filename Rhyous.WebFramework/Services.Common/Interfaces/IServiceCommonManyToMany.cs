using System.Collections.Generic;

namespace Rhyous.WebFramework.Services
{
    public interface IServiceCommonManyToMany<T,Tinterface> : IServiceCommon<T, Tinterface>
        where T : class, Tinterface
    {
        string PrimaryEntity { get; }
        string SecondaryEntity { get; }
        string IdSuffix { get; }
        List<Tinterface> GetByRelatedEntityId(int id, string entity);
    }
}