using System;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class WebServiceTypesModel
    {
        public Type EntityType { get; set; }
        public Type InterfaceType { get; set; }
        public Type IdType { get; set; }
        public Type WebServiceGenericType { get; set; }
        public Type LoaderType { get; set; }
        public Type[] AdditionalWebServiceTypes { get; set; }
    }
}