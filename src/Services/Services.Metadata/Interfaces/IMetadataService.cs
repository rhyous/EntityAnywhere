using Rhyous.Odata.Csdl;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IMetadataService
    {
        CsdlEntity GetCsdlEntity(Type type);
        CsdlSchema GetCsdlSchema(IEnumerable<Type> types);
        CsdlDocument GetCsdlDocument(IEnumerable<Type> type);
    }
}