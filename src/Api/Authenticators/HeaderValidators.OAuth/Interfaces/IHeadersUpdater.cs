using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    public interface IHeadersUpdater
    {
        void Update(IAccessToken token, IHeadersContainer headers);
    }
}