using Rhyous.Wrappers;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public interface IAccessController
    {
        bool IsAnonymousAllowed(string absolutePath);
        bool IsSystemAdmin(NameValueCollection headers, IWebOperationContext webContext);
    }
}