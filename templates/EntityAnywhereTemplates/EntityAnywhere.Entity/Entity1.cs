using System;
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    public class Entity1 : AuditableEntityBase<int>, IEntity1
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
