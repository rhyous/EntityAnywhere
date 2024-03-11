using Rhyous.Odata.Csdl;
using Rhyous.Collections;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class MetadataCache : CacheBase<IMetadataDictionary>, IMetadataCache
    {
        const string EAF = nameof(EAF);
        private const string TargetOdataSpecVersion = "4.01";
        private readonly ICustomMetadataProvider _CustomMetadataProvider;
        private readonly IEntityList _EntityList;
        private CsdlDocument _CsdlDocument;
        private CsdlSchema _CsdlSchema;

        public MetadataCache(ICustomMetadataProvider customMetadataProvider,
                             IEntityList entityList)
            : base(new MetadataDictionary())
        {
            _CustomMetadataProvider = customMetadataProvider;
            _EntityList = entityList;
        }


        public override void Clear()
        {
            _CsdlDocument = null;
            _CsdlSchema = null;
            base.Clear();
        }

        protected override Task CreateCacheAsync()
        {
            GetCsdlDocument();
            return Task.CompletedTask;
        }

        public CsdlDocument GetCsdlDocument(bool forceUpdate = false)
        {
            if (!forceUpdate && _CsdlDocument != null)
                return _CsdlDocument;
            var csdlDocument = new CsdlDocument() { EntityContainer = EAF, Version = TargetOdataSpecVersion };
            _CsdlSchema = GetCsdlSchema(forceUpdate);
            csdlDocument.Schemas.Add(EAF, _CsdlSchema);
            return csdlDocument;
        }

        public CsdlSchema GetCsdlSchema(bool forceUpdate = false)
        {
            if (!forceUpdate && _CsdlSchema != null)
                return _CsdlSchema;
            _CsdlSchema = new CsdlSchema();
            foreach (var entityType in _EntityList.Entities.OrderBy(t => t.Name))
            {
                var csdl = GetCsdlEntity(entityType, forceUpdate);
                _CsdlSchema.Entities.AddOrUpdate(entityType.Name, t => csdl, (t, c) => csdl);
            }
            return _CsdlSchema;
        }

        public CsdlEntity GetCsdlEntity(Type type, bool forceUpdate = false)
        {
            // Check cache
            if (!_Cache.TryGetValue(type, out CsdlEntity csdl) || forceUpdate)
            {
                csdl = _CustomMetadataProvider.Provide(type);
                // Cache it if not in cache
                _Cache.AddOrUpdate(type, t => csdl, (t, c) => csdl);
            }
            return csdl;
        }
    }
}