using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>An attribute to property name the controller.</summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class EntityGenericControllerAttribute : Attribute, IControllerModelConvention
    {
        /// <summary>Called to apply the convention of {EntityName}Service to the <see cref="Microsoft.AspNetCore.Mvc.ApplicationModels.ControllerModel"/>.</summary>
        public void Apply(ControllerModel controller)
        {
            var attribute = controller.ControllerType.GetAttribute<CustomControllerAttribute>();
            if (attribute != null && attribute.Entity != null)
            {
                controller.ControllerName = attribute.Entity.Name + "Service";
                return;
            }
            if (!controller.ControllerType.IsGenericType)
            {
                return;
            }
            var entityType = controller.ControllerType.GenericTypeArguments[0];
            controller.ControllerName = entityType.Name + "Service";
        }
    }
}