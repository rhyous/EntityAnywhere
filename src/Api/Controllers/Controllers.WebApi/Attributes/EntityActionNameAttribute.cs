namespace Rhyous.EntityAnywhere.Attributes
{
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Rhyous.StringLibrary.Pluralization;
    using System;

    /// <summary>
    /// An attribute that preovides the template: {EntityName}Service/{EntityNamePluralized}
    /// Example: UserService/Users
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class EntityActionNameAttribute : Attribute, IActionModelConvention
    {
        public string ActionNameTemplate { get; set; }
        public Type? EntityType { get; set; }
        public string? PluralizedEntityName { get; set; }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="actionNameTemplate">A template.</param>
        public EntityActionNameAttribute(string actionNameTemplate)
        {
            ActionNameTemplate = actionNameTemplate;
        }

        /// <summary>
        /// Called to apply the convention to the Microsoft.AspNetCore.Mvc.ApplicationModels.ActionModel.
        /// </summary>
        /// <param name="actionModel">The <see cref="Microsoft.AspNetCore.Mvc.ApplicationModels.ActionModel"/>.</param>
        public void Apply(ActionModel actionModel)
        {
            if (string.IsNullOrWhiteSpace(PluralizedEntityName))
            {
                var attribute = actionModel.Controller.ControllerType.GetAttribute<CustomControllerAttribute>();
                EntityType = attribute?.Entity ?? actionModel.Controller.ControllerType.GenericTypeArguments[0];
                PluralizedEntityName = EntityType.Name.Pluralize();
            }
            actionModel.ActionName = ActionNameTemplate.Replace("{EntityPluralized}", PluralizedEntityName);
        }
    }
}
