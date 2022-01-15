using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Services
{
    public class MetadataService : IMetadataService, ILogProperty
    {
        const string EAF = "EAF";

        private readonly IMetadataCache _MetadataCache;
        private readonly ICustomMetadataProvider _CustomMetadatProvider;

        public MetadataService(ICustomMetadataProvider customMetadataProvider,
                               IMetadataCache metadataCache,
                               ILogger logger)
        {
            _CustomMetadatProvider = customMetadataProvider ?? throw new ArgumentNullException(nameof(customMetadataProvider));
            _MetadataCache = metadataCache ?? throw new ArgumentNullException(nameof(metadataCache));
            Logger = logger;
        }

        public ILogger Logger { get; set; }

        public CsdlEntity GetCsdlEntity(Type type)
        {
            // Check cache
            if (!_MetadataCache.EntityMetadata.TryGetValue(type, out CsdlEntity csdl))
            {
                csdl = _CustomMetadatProvider.Provide(type);
                // Cache it if not in cache
                _MetadataCache.EntityMetadata.GetOrAdd(type, csdl);
            }
            return csdl;
        }

        public CsdlDocument GetCsdlDocument(IEnumerable<Type> types)
        {
            var csdlDocument = new CsdlDocument() { EntityContainer = EAF, Version = "4.01" };
            var csdlSchema = GetCsdlSchema(types);
            csdlDocument.Schemas.Add(EAF, csdlSchema);
            return csdlDocument;
        }

        public CsdlSchema GetCsdlSchema(IEnumerable<Type> types)
        {
            var csdlSchema = new CsdlSchema();
            foreach (var entityType in types.OrderBy(t => t.Name))
            {
                var entityCsdl = GetCsdlEntity(entityType);
                csdlSchema.Entities.Add(entityType.Name, entityCsdl);
            }
            return csdlSchema;
        }
    }
}
