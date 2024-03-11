using Rhyous.EntityAnywhere.Attributes;
using System.Reflection;

namespace Rhyous.EntityAnywhere.WebApi
{
    public class EntityActionNameAttributeUpdater : IEntityActionNameAttributeUpdater
    {
        public void Update(Type controllerType, Type entityType)
        {
            var methods = controllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            foreach (var mi in methods)
            {
                var attribute = mi.GetCustomAttribute<EntityActionNameAttribute>();
                if (attribute == null)
                    continue;
                attribute.EntityType = entityType;
            }
        }
    }
}
