using System;
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class $ext_Entity$ : AuditableEntity<int>, I$ext_Entity$
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
