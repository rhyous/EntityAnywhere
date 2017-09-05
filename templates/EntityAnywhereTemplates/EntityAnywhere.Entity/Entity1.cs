using System;
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Entity1 : AuditableEntity<int>, IEntity1
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
