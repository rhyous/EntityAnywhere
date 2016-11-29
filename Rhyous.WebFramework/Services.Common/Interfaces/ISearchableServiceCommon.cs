using System.Collections.Generic;

namespace Rhyous.WebFramework.Services
{
    public interface ISearchableServiceCommon<T, Tinterface> : IServiceCommon<T,Tinterface>
        where T : class
    {
        Tinterface Get(string name);
        List<Tinterface> Search(string name);
    }
}
