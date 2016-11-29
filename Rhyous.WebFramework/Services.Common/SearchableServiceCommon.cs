using System.Collections.Generic;

namespace Rhyous.WebFramework.Services
{
    public abstract class SearchableServiceCommon<T, Tinterface> 
        : ServiceCommon<T, Tinterface>, ISearchableServiceCommon<T, Tinterface>
        where T: class
    {
        public abstract Tinterface Get(string name);
        public abstract List<Tinterface> Search(string name);
    }
}