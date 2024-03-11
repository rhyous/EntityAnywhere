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
    public class MappingEntityActionNameAttribute : Attribute, IActionModelConvention
    {
        public string ActionNameTemplate { get; set; }
        public Type? EntityType { get; set; }
        public string? PluralizedEntityName { get; set; }
        public Type? E1Type { get; set; }
        public string? PluralizedE1Name { get; set; }
        public Type? E2Type { get; set; }
        public string? PluralizedE2Name { get; set; }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="actionNameTemplate">A template.</param>
        public MappingEntityActionNameAttribute(string actionNameTemplate)
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
                EntityType = EntityType ?? actionModel.Controller.ControllerType.GenericTypeArguments[0];
                PluralizedEntityName = EntityType.Name.Pluralize();
            }
            if (string.IsNullOrWhiteSpace(PluralizedE1Name))
            {
                E1Type = E1Type ?? actionModel.Controller.ControllerType.GenericTypeArguments[3];
                PluralizedE1Name = E1Type.Name.Pluralize();
            }
            if (string.IsNullOrWhiteSpace(PluralizedE2Name))
            {
                E2Type = E2Type ?? actionModel.Controller.ControllerType.GenericTypeArguments[4];
                PluralizedE2Name = E2Type.Name.Pluralize();
            }
            actionModel.ActionName = ActionNameTemplate.Replace("{EntityPluralized}", PluralizedEntityName) 
                                                       .Replace("{E1Pluralized}", PluralizedE1Name)
                                                       .Replace("{E2Pluralized}", PluralizedE2Name);
        }
    }
}
