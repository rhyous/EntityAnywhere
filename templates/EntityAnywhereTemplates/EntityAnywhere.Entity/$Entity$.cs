using System;
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class $Entity$ : AuditableEntity<int>, $IEntity$
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
