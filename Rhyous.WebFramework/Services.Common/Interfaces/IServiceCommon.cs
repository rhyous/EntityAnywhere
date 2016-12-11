using Rhyous.WebFramework.Interfaces;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Services
{
    public interface IServiceCommon<T, Tinterface, Tid>
        where T : class, Tinterface
    {
        IRepository<T, Tinterface, Tid> Repo { get; set; }
        List<Tinterface> Get();
        List<Tinterface> Get(List<Tid> ids);
        Tinterface Get(Tid id);
        string GetProperty(Tid id, string property);
        Tinterface Update(Tid id, Tinterface entity, List<string> changedProperties);
        List<Tinterface> Add(IList<Tinterface> entity);
        Tinterface Replace(Tid id, Tinterface entity);
        bool Delete(Tid userId);
    }
}