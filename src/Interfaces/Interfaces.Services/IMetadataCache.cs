using Rhyous.Odata.Csdl;
using System;
using System.Collections.Concurrent;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IMetadataCache
    {
        ConcurrentDictionary<Type, CsdlEntity> EntityMetadata { get; }
    }
}