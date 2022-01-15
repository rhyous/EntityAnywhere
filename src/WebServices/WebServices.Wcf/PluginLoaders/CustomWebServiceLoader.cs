using Autofac;
using Rhyous.SimplePluginLoader;
using Rhyous.SimplePluginLoader.DependencyInjection;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Linq;

namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// This loads custom web services. These web services are usually not Entity services.
    /// </summary>
    public class CustomWebServiceLoader : RuntimePluginLoaderBase<ICustomWebService>, ICustomWebServiceLoader
    {
        private readonly IEndpointBuilder _EndpointBuilder;
        private readonly ILoadedEntitiesTracker _LoadedEntitiesTracker;
        private readonly IPluginLoader<IDependencyRegistrar<ContainerBuilder>> _DependencyRegistrarPluginLoader;
        private readonly IPluginObjectCreator<IDependencyRegistrar<ContainerBuilder>> _DependencyRegistrarPluginObjectCreator;
        private readonly ContainerBuilder _ContainerBuilder;
        private readonly ILogger _Logger2;

        public CustomWebServiceLoader(IEndpointBuilder endpointBuilder,
                                      ILoadedEntitiesTracker loadedEntitiesTracker,
                                      IPluginLoader<IDependencyRegistrar<ContainerBuilder>> dependencyRegistrarPluginLoader,
                                      IPluginObjectCreator<IDependencyRegistrar<ContainerBuilder>> dependencyRegistrarPluginObjectCreator,
                                      ContainerBuilder containerBuilder,
                                      IAppDomain appDomain,
                                      IPluginLoaderSettings settings,
                                      IPluginLoaderFactory<ICustomWebService> pluginLoaderFactory,
                                      IPluginObjectCreator<ICustomWebService> pluginObjectCreator,
                                      IPluginPaths pluginPaths = null,
                                      IPluginLoaderLogger pluginLoaderLogger = null,
                                      ILogger logger2 = null)
            : base(appDomain, settings, pluginLoaderFactory, pluginObjectCreator, pluginPaths, pluginLoaderLogger)
        {
            _EndpointBuilder = endpointBuilder;
            _LoadedEntitiesTracker = loadedEntitiesTracker;
            _DependencyRegistrarPluginLoader = dependencyRegistrarPluginLoader;
            _DependencyRegistrarPluginObjectCreator = dependencyRegistrarPluginObjectCreator;
            _ContainerBuilder = containerBuilder;
            _Logger2 = logger2;
        }


        /// <summary>
        /// The directory of the plugins.
        /// </summary>
        public override string PluginSubFolder => "WebServices";

        public void Load()
        {
            if (_Logger2 == null) throw new ArgumentNullException(nameof(_Logger2));
            if (PluginTypes == null)
                return;
            var customWebServiceTypes = PluginTypes.Where(t => !t.IsInterface).ToList();
            foreach (var plugin in PluginCollection)
            {
                var registrarPlugin  = _DependencyRegistrarPluginLoader.LoadPlugin(plugin.FullPath);
                var pluginObjects = registrarPlugin.CreatePluginObjects(_DependencyRegistrarPluginObjectCreator);
                if (pluginObjects != null && pluginObjects.Any())
                {
                    foreach (var registrarObject in pluginObjects)
                    {
                        registrarObject.Register(_ContainerBuilder);
                    }
                }
            }
            foreach (Type customWebServiceType in customWebServiceTypes)
            {
                var attribute = customWebServiceType.GetCustomAttributes(true)?.FirstOrDefault(a => typeof(CustomWebServiceAttribute).IsAssignableFrom(a.GetType())) as CustomWebServiceAttribute;
                if (attribute != null)
                {
                    _EndpointBuilder.BuildCustomEndpoint(customWebServiceType, attribute);
                    if (attribute.Entity != null)
                    {
                        _LoadedEntitiesTracker.Entities.Add(attribute.Entity);
                    }
                    else
                    {
                        _Logger2?.Write($"{customWebServiceType} custom non-entity endpoint loaded.");
                    }
                }
            }
        }
    }
}