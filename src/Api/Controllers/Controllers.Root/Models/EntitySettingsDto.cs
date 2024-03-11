using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class EntitySettingsDto : Entity
    {
        public string SortByProperty { get; set; }
        public string EntityGroup { get;set;}

        public EntityPropertyDictionary EntityProperties { get; set; }
    }
}