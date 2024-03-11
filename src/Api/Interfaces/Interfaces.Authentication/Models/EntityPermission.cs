using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Rhyous.EntityAnywhere.Interfaces
{
    [ExcludeFromCodeCoverage]
    public class EntityPermission
    {
        public string Entity { get; set; }
        public HashSet<string> Permissions { get; set; }
    }
}