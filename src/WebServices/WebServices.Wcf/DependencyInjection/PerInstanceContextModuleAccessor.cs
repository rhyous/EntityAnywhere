using Autofac.Core;
using Autofac.Integration.Wcf;
using Rhyous.EntityAnywhere.Clients2.DependencyInjection;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services.RelatedEntities;
using Rhyous.EntityAnywhere.WebServices.DependencyInjection;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.WebServices

{
    public class PerInstanceContextModuleAccessor : IPerInstanceContextModuleAccessor
    {
        public PerInstanceContextModuleAccessor(IEntityList entityList,
                                                IExtensionEntityList extensionEntityList,
                                                IMappingEntityList mappingEntityList)
        {
            Modules = new IModule[]
            {
                new WcfPerInstanceContextModule(),
                new RestHandlerRegistrationModule(),
                new EntityClientPerCallModule(entityList, extensionEntityList, mappingEntityList),
                new RelatedEntityRegistrationModule()
            };
        }

        public IEnumerable<IModule> Modules { get; }
    }
}