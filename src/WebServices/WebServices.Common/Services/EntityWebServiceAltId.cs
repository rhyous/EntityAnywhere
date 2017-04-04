using Rhyous.WebFramework.Interfaces;
using System;
using System.Linq;

namespace Rhyous.WebFramework.WebServices
{
    public class EntityWebServiceAltId<T, Tinterface, Tid, TService> : EntityWebService<T, Tinterface, Tid, TService>
        where T : class, Tinterface, new()
        where Tinterface : IEntity<Tid>
        where TService : class, IServiceCommonAltId<T, Tinterface, Tid>, new()
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
    {
        public override OdataObject<T> Get(string idOrName)
        {
            if (string.IsNullOrWhiteSpace(idOrName))
                return null;
            if (idOrName.Any(c => !char.IsDigit(c)))
                return AltIdService.Get(idOrName)?.ToConcrete<T, Tinterface>().AsOdata(RequestUri, GetAddenda(idOrName));
            return base.Get(idOrName);
        }
        
        public virtual IServiceCommonAltId<T, Tinterface, Tid> AltIdService
        {
            get { return Service as IServiceCommonAltId<T, Tinterface, Tid>; }
        }
    }
}
