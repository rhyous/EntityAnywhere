using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Events
{
	public class $Entity$Event : IEntityEventBefore<$Entity$>, IEntityEventAfter<$Entity$>, ILogProperty
    {
        public ILogger Logger { get; set; }

        public void BeforePatch(PatchedEntity<$Entity$> patchedEntity, $Entity$ currentEntity)
        {
        }

        public void BeforePost(List<$Entity$> postedItems)
        {
        }

        public void BeforePut(Entity newEntity, $Entity$ existingEntity)
        {
        }

        public void BeforeUpdateProperty(string property, object newValue, object existingValue)
        {
        }

        public void BeforeDelete($Entity$ entity)
        {
        }

        public void AfterDelete($Entity$ entity, bool wasDeleted)
        {
        }

        public void AfterPatch(PatchedEntity<$Entity$> patchedEntity, $Entity$ priorEntity)
        {
        }

        public void AfterPost(IEnumerable<$Entity$> postedItems)
        {
        }

        public void AfterPut(Entity newEntity, $Entity$ priorEntity)
        {
        }

        public void AfterUpdateProperty(string property, object newValue, object existingValue)
        {
        }
    }
}
