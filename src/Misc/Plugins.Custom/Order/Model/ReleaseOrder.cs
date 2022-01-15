using System.Diagnostics.CodeAnalysis;

namespace Rhyous.WebFramework.Services
{
    [ExcludeFromCodeCoverage]
    public class ReleaseOrder
    {
        public string OrderId { get; set; }
        public string[] OrderLines { get; set; }
    }
}
