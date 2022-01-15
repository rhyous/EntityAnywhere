using Rhyous.Odata.Csdl;
using System;
using System.Collections.Concurrent;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class MetadataCache : IMetadataCache
    {
        public ConcurrentDictionary<Type, CsdlEntity> EntityMetadata
        {
            get { return _EntityMetadata ?? (_EntityMetadata = new ConcurrentDictionary<Type, CsdlEntity>()); }
            internal set { _EntityMetadata = value; }
        } private ConcurrentDictionary<Type, CsdlEntity> _EntityMetadata;
    }
}