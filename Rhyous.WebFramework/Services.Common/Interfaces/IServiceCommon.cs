using Rhyous.WebFramework.Interfaces;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Services
{
    public interface IServiceCommon<T, Tinterface>
        where T : class, Tinterface
    {
        IRepository<T, Tinterface> Repo { get; set; }
        List<Tinterface> Get();
        List<Tinterface> Get(List<int> ids);
        Tinterface Get(int id);
        string GetProperty(int id, string property);
        Tinterface Update(int id, Tinterface item, List<string> changedProperties);
        List<Tinterface> Add(IList<Tinterface> item);
        Tinterface Replace(int userId, Tinterface user);
        bool Delete(int userId);
    }
}