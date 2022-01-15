using Rhyous.Collections;
using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Linq;

namespace Rhyous.EntityAnywhere.Services
{
    /// <summary>
    /// This is a class where all custom Metadata can be provided on top of 
    /// what Rhyous.Odata already provides.
    /// </summary>
    internal class CustomMetadataProvider : ICustomMetadataProvider
    {
        private readonly IExtensionEntityList _ExtensionEntityList;
        private readonly ICsdlBuilderFactory _CsdlBuilderFactory;

        public CustomMetadataProvider(ICsdlBuilderFactory csdlBuilderFactory, 
                                      IExtensionEntityList extensionEntityList, 
                                      IFuncList<string> propertyFuncs,
                                      IFuncList<string, string> propertyDataFuncs)
        {
            _CsdlBuilderFactory = csdlBuilderFactory ?? throw new ArgumentNullException(nameof(csdlBuilderFactory));
            _ExtensionEntityList = extensionEntityList;
            if (propertyFuncs != null && propertyFuncs.Any())
                _CsdlBuilderFactory.CustomPropertyFuncs.AddRange(propertyFuncs);
            if (propertyFuncs != null && propertyDataFuncs.Any())
                _CsdlBuilderFactory.CustomPropertyDataFuncs.AddRange(propertyDataFuncs);
            _CsdlBuilderFactory.EntityAttributeDictionary.AddIfNewAndNotNull(typeof(DisplayNamePropertyAttribute), new DisplayNamePropertyFunction().GetDisplayNameProperty);
        }

        /// <summary>
        /// A provide for $Metadata
        /// </summary>
        /// <returns>The metadata for a given type.</returns>
        public CsdlEntity Provide(Type type)
        {
            var csdl = _CsdlBuilderFactory.EntityBuilder.Build(type);
            Customize(csdl, type);
            return csdl;
        }

        /// <summary>
        /// Provide customizations that aren't attribute related.
        /// Anything you can code goes here.
        /// </summary>
        /// <param name="csdl">A CsdlEntity.</param>
        /// <param name="entityType">The type of the entity.</param>
        /// <remarks>You should try to use a property attribute before using this.</remarks>
        public void Customize(CsdlEntity csdl, Type entityType)
        {
            // Metadata for Extension Entities such as Addenda and AlternateIds.
            csdl.AddExtensionEntityNavigationProperties(entityType, _ExtensionEntityList.Entities);

            // AlternateKey
            csdl.AddAlternateKey(entityType);
        }
    }
}