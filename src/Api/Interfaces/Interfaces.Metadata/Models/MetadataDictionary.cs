using Rhyous.Odata.Csdl;
using System;
using System.Collections.Concurrent;

namespace Rhyous.EntityAnywhere.Interfaces
{
    internal class MetadataDictionary : ConcurrentDictionary<Type, CsdlEntity>, IMetadataDictionary
    {
    }
}