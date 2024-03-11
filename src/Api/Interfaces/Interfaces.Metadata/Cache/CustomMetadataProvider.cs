using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Linq;

namespace Rhyous.EntityAnywhere.Cache
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
                                      ICustomPropertyFuncs propertyFuncs,
                                      ICustomPropertyDataFuncs propertyDataFuncs,
                                      IDisplayNamePropertyFunction displayNamePropertyFunction)
        {
            _CsdlBuilderFactory = csdlBuilderFactory ?? throw new ArgumentNullException(nameof(csdlBuilderFactory));
            _ExtensionEntityList = extensionEntityList;
            if (propertyFuncs != null && propertyFuncs.Any())
            {
                foreach (var func in propertyFuncs)
                {
                    if (!_CsdlBuilderFactory.CustomPropertyFuncs.Contains(func))
                        _CsdlBuilderFactory.CustomPropertyFuncs.Add(func);
                }
            }
            if (propertyDataFuncs != null && propertyDataFuncs.Any())
            {
                foreach (var func in propertyDataFuncs)
                {
                    if (!_CsdlBuilderFactory.CustomPropertyDataFuncs.Contains(func))
                        _CsdlBuilderFactory.CustomPropertyDataFuncs.Add(func);
                }
            }            
            _CsdlBuilderFactory.EntityAttributeDictionary.TryAdd(typeof(DisplayNamePropertyAttribute), displayNamePropertyFunction.GetDisplayNameProperty);
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

            // File Upload
            csdl.AddFileUpload(entityType);
        }
    }
}