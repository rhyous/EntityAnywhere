using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IAppSettings
    {
        NameValueCollection Collection { get; }
    }
}
