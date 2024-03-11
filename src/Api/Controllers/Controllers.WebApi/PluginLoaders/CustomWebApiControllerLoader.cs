using Autofac;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.WebServices;
using Rhyous.SimplePluginLoader;
using Rhyous.SimplePluginLoader.DependencyInjection;
using ILogger = Rhyous.EntityAnywhere.Interfaces.ILogger;

namespace Rhyous.EntityAnywhere.WebApi
{
    internal class CustomWebApiControllerLoader : RuntimePluginLoaderBase<ICustomWebService>, ICustomWebServiceLoader
    {
        private readonly ILoadedEntitiesTracker _LoadedEntitiesTracker;
        private readonly IPluginLoader<IDependencyRegistrar<ContainerBuilder>> _DependencyRegistrarPluginLoader;
        private readonly IPluginObjectCreator<IDependencyRegistrar<ContainerBuilder>> _DependencyRegistrarPluginObjectCreator;
        private readonly IWebApiScopeContainerBuilder _WebApiScopeContainerBuilder;
        private readonly IEntityControllerList _EntityControllerList;
        private readonly Interfaces.ILogger _Logger2;

        public CustomWebApiControllerLoader(ILoadedEntitiesTracker loadedEntitiesTracker,
                                            IPluginLoader<IDependencyRegistrar<ContainerBuilder>> dependencyRegistrarPluginLoader,
                                            IPluginObjectCreator<IDependencyRegistrar<ContainerBuilder>> dependencyRegistrarPluginObjectCreator,
                                            IWebApiScopeContainerBuilder webApiScopeContainerBuilder,
                                            IEntityControllerList entityControllerList,
                                            IAppDomain appDomain,
                                            IPluginLoaderSettings settings,
                                            IPluginLoaderFactory<ICustomWebService> pluginLoaderFactory,
                                            IPluginObjectCreator<ICustomWebService> pluginObjectCreator,
                                            IPluginPaths pluginPaths,
                                            IPluginLoaderLogger pluginLoaderLogger,
                                            ILogger logger2)
            : base(appDomain, settings, pluginLoaderFactory, pluginObjectCreator, pluginPaths, pluginLoaderLogger)
        {
            _LoadedEntitiesTracker = loadedEntitiesTracker;
            _DependencyRegistrarPluginLoader = dependencyRegistrarPluginLoader;
            _DependencyRegistrarPluginObjectCreator = dependencyRegistrarPluginObjectCreator;
            _WebApiScopeContainerBuilder = webApiScopeContainerBuilder;
            _EntityControllerList = entityControllerList;
            _Logger2 = logger2;
        }

        public override string PluginSubFolder => "Controllers"; // User Controllers folder to differentiate between WCF

        public void Load()
        {
            if (PluginTypes == null)
                return;
            var customControllerTypes = PluginTypes.Where(t => !t.IsInterface).ToList();
            foreach (var plugin in PluginCollection)
            {
                var registrarPlugin = _DependencyRegistrarPluginLoader.LoadPlugin(plugin.FullPath);
                var pluginObjects = registrarPlugin.CreatePluginObjects(_DependencyRegistrarPluginObjectCreator);
                if (pluginObjects != null && pluginObjects.Any())
                {
                    foreach (var registrarObject in pluginObjects)
                    {
                        registrarObject.Register(_WebApiScopeContainerBuilder.ContainerBuilder);
                    }
                }
            }
            foreach (Type customControllerType in customControllerTypes)
            {
                _EntityControllerList.ControllerTypes.Add(customControllerType);
                _WebApiScopeContainerBuilder.Register(customControllerType);
                var attribute = customControllerType.GetCustomAttributes(true)?.FirstOrDefault(a => typeof(CustomControllerAttribute).IsAssignableFrom(a.GetType())) as CustomControllerAttribute;
                if (attribute != null)
                {
                    if (attribute.Entity != null)
                    {
                        _LoadedEntitiesTracker.Entities.Add(attribute.Entity);
                    }
                    else
                    {
                        _Logger2?.Write($"{customControllerType} custom controller endpoint loaded.");
                    }
                }
            }
        }
    }
}
