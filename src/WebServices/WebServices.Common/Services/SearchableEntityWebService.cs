using Rhyous.StringLibrary;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Linq;

namespace Rhyous.WebFramework.WebServices
{
    public class SearchableEntityWebService<T, Tinterface, Tid, TService> : EntityWebService<T, Tinterface, Tid, TService>
        where T : class, Tinterface, new()
        where Tinterface : IEntity<Tid>
        where TService : class, ISearchableServiceCommon<T, Tinterface, Tid>, new()
        where Tid : IComparable, IConvertible, IComparable<Tid>, IEquatable<Tid>
    {
        public override OdataObject<T> Get(string idOrName)
        {
            if (string.IsNullOrWhiteSpace(idOrName))
                return null;
            if (idOrName.Any(c => !char.IsDigit(c)))
                return SearchableService.Get(idOrName)?.ToConcrete<T, Tinterface>().AsOdata(GetRequestUri());
            return Service.Get(idOrName.To<Tid>())?.ToConcrete<T, Tinterface>().AsOdata(GetRequestUri());
        }
        
        public virtual ISearchableServiceCommon<T, Tinterface, Tid> SearchableService
        {
            get { return Service as ISearchableServiceCommon<T, Tinterface, Tid>; }
        }
    }
}
