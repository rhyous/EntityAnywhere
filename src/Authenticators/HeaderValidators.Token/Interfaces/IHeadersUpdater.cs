using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    public interface IHeadersUpdater
    {
        void Update(IToken token, NameValueCollection headers);
    }
}