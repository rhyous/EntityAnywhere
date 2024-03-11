using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Rhyous.Collections;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.StringLibrary.Pluralization;
using System.Reflection;

namespace Rhyous.EntityAnywhere.WebApi
{
    public class EntityControllerFeatureProvider : IEntityControllerFeatureProvider
    {
        IEntityControllerList _EntityControllerList;
        public EntityControllerFeatureProvider(IEntityControllerList entityControllerList)
        {
            _EntityControllerList = entityControllerList;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            feature.Controllers.AddRange(_EntityControllerList.ControllerTypes.Select(t => t.GetTypeInfo()));
        }
    }
}