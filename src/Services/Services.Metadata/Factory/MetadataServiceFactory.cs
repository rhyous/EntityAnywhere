using Autofac;
using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Tools;
using System;

namespace Rhyous.EntityAnywhere.Services
{
    public class MetadataServiceFactory : IMetadataServiceFactory
    {
        private readonly IExtensionEntityList _ExtensionEntityList;
        private readonly IMetadataCache _MetadataCache;
        private readonly IEntitySettingsProvider _EntitySettingsProvider;
        private readonly ILogger _Logger;

        public MetadataServiceFactory(
            IExtensionEntityList extensionEntityList,
            IMetadataCache metadataCache,
            IEntitySettingsProvider entitySettingsProvider,
            ILogger logger = null
        )
        {
            _ExtensionEntityList = extensionEntityList ?? throw new ArgumentNullException(nameof(extensionEntityList));
            _MetadataCache = metadataCache ?? throw new ArgumentNullException(nameof(metadataCache));
            _EntitySettingsProvider = entitySettingsProvider;
            _Logger = logger; // Nullable
        }

        public IMetadataService MetadataService => CreateService();

        public ICustomMetadataProvider CustomMetadataProvider => CreateCustomMetadataProvider();

        internal IMetadataService CreateService() => new MetadataService(CustomMetadataProvider, _MetadataCache, _Logger);

        internal ICustomMetadataProvider CreateCustomMetadataProvider()
        {
            var entitySettings = TaskRunner.RunSynchonously(_EntitySettingsProvider.GetAsync);
            var propertyFuncProvider = new PropertyFuncProvider(entitySettings);
            var propertyFuncs = propertyFuncProvider.Provide();
            var propertyDataFuncProvider = new PropertyDataFuncProvider(entitySettings);
            var propertyDataFuncs = propertyDataFuncProvider.Provide();
            var csdlBuilderFactory = new CsdlBuilderFactory();

            var hrefPropertyDataAttributeProvider = new HrefPropertyDataAttributeFuncProvider();
            csdlBuilderFactory.PropertyDataAttributeDictionary.Add(typeof(HRefAttribute), hrefPropertyDataAttributeProvider.Func);


            return new CustomMetadataProvider(csdlBuilderFactory, 
                                              _ExtensionEntityList, 
                                              propertyFuncs, 
                                              propertyDataFuncs);
        }
    }
}