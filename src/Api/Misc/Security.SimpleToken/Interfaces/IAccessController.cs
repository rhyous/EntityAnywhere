using Microsoft.AspNetCore.Http;
using Rhyous.Wrappers;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Security
{
    public interface IAccessController
    {
        bool IsAnonymousAllowed(string absolutePath);
        bool IsSystemAdmin(IHeaderDictionary headers);
    }
}