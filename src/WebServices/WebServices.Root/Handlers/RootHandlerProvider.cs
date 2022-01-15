using Autofac;
using System;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class RootHandlerProvider : IRootHandlerProvider
    {
        protected readonly ILifetimeScope _Container;

        public RootHandlerProvider(ILifetimeScope container)
        {
            _Container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public IEntitySettingsHandler EntitySettingsHandler => _Container.Resolve<IEntitySettingsHandler>();
        public IGenerateHandler GenerateHandler => _Container.Resolve<IGenerateHandler>();
        public IGetMetadataHandler GetMetadataHandler => _Container.Resolve<IGetMetadataHandler>();
        public ISeedEntityHandler SeedEntityHandler => _Container.Resolve<ISeedEntityHandler>();
    }
}
