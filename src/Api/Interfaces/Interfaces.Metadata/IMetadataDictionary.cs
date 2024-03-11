using Rhyous.Collections;
using Rhyous.Odata.Csdl;
using System;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IMetadataDictionary : IConcurrentDictionary<Type, CsdlEntity>, IClearable, ICountable
    {
    }
}