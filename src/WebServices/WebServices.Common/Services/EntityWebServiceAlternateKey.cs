using Rhyous.WebFramework.Interfaces;
using System;
using System.Linq;

namespace Rhyous.WebFramework.WebServices
{
    public class EntityWebServiceAlternateKey<T, Tinterface, Tid, TService> : EntityWebService<T, Tinterface, Tid, TService>
        where T : class, Tinterface, new()
        where Tinterface : IEntity<Tid>
        where TService : class, IServiceCommonAlternateKey<T, Tinterface, Tid>, new()
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
    {
        public override OdataObject<T> Get(string idOrName)
        {
            if (string.IsNullOrWhiteSpace(idOrName))
                return null;
            if (idOrName.Any(c => !char.IsDigit(c)))
                return AltKeyService.Get(idOrName)?.ToConcrete<T, Tinterface>().AsOdata(RequestUri, GetAddenda(idOrName));
            return base.Get(idOrName);
        }
        
        public virtual IServiceCommonAlternateKey<T, Tinterface, Tid> AltKeyService
        {
            get { return Service as IServiceCommonAlternateKey<T, Tinterface, Tid>; }
        }
    }
}
