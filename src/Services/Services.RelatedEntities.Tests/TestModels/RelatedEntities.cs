using Rhyous.Odata;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities.Tests
{
    public interface IA : IBaseEntity<int>
    {
        string Name { get; set; }
        int BId { get; set; }
    }

    public class A : BaseEntity<int>, IA
    {
        public string Name { get; set; }
        [RelatedEntity(nameof(B))]
        public int BId { get; set; }
    }

    public interface IB : IId<int>
    {
        string Name { get; set; }
    }

    [RelatedEntityForeign(nameof(A), nameof(B))]
    public class B : IB
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
