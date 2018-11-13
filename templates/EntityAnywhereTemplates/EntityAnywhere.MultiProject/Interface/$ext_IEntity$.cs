using System;

namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface $ext_IEntity$ : IEntity<int>, IName, IDescription, IAuditable
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
