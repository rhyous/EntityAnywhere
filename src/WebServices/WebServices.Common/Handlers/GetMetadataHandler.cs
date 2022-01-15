using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class GetMetadataHandler : IGetMetadataHandler
    {
        private readonly IMetadataServiceFactory _MetadataServiceFactory;

        public GetMetadataHandler(IMetadataServiceFactory metadataServiceFactory)
        {
            _MetadataServiceFactory = metadataServiceFactory 
                                    ?? throw new ArgumentNullException(nameof(metadataServiceFactory));
        }

        public Task<CsdlEntity> Handle(Type type)
        {
            return Task.FromResult(_MetadataServiceFactory.MetadataService.GetCsdlEntity(type));
        }

        public Task<CsdlDocument> Handle(IEnumerable<Type> types)
        {
            return Task.FromResult(_MetadataServiceFactory.MetadataService.GetCsdlDocument(types));
        }
    }
}