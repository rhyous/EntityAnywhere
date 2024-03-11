using Rhyous.EntityAnywhere.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Services
{
    public class MissingEntitySettings
    {
        public Dictionary<string, Missing<EntityWithMissingProperties>> Entities
        {
            get => _Entities ?? (_Entities = new Dictionary<string, Missing<EntityWithMissingProperties>>());
            set => _Entities = value;
        } private Dictionary<string, Missing<EntityWithMissingProperties>> _Entities;

        public Dictionary<string, Missing<EntityGroup>> EntityGroups
        {
            get => _EntityGroups ?? (_EntityGroups = new Dictionary<string, Missing<EntityGroup>>());
            set => _EntityGroups = value;
        } private Dictionary<string, Missing<EntityGroup>> _EntityGroups;

        public bool Any()
        {
            return Entities.Any()
                    && (Entities.Any(e => e.Value.IsMissing)
                        || Entities.Any(e => e.Value.Object.EntityProperties.Any()));
        }
    }
}
