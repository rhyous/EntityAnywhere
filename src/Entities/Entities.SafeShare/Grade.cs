using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;

namespace Entities.SafeShare
{
    [GradeSeedData]
    public class Grade : AuditableEntity<int>, IGrade
    {
        public string Name { get; set; }
        public int Year { get; set; }
    }

    public interface IGrade : IBaseEntity<int>, IAuditable, IName
    {
        int Year { get; set; }
    }
}