using Rhyous.Collections;
using Rhyous.Odata.Csdl;
using System;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IMetadataCache : ICacheBase<IMetadataDictionary>
    {
        CsdlDocument GetCsdlDocument(bool forceUpdate = false);
        CsdlSchema GetCsdlSchema(bool forceUpdate = false);
        CsdlEntity GetCsdlEntity(Type type, bool forceUpdate = false);
    }
}