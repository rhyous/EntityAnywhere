using Rhyous.Collections;
using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class GetMetadataHandler : IGetMetadataHandler
    {
        private readonly IMetadataCache _MetadataCache;
        private readonly IUrlParameters _UrlParameters;

        public GetMetadataHandler(IMetadataCache MetadataCache,
                                  IUrlParameters urlParameters)
        {
            _MetadataCache = MetadataCache;
            _UrlParameters = urlParameters;
        }

        private bool ForceUpdate => _UrlParameters.Collection.Get("ForceUpdate", false);

        public Task<CsdlEntity> Handle(Type type)
        {
            return Task.FromResult(_MetadataCache.GetCsdlEntity(type, ForceUpdate));
        }

        public Task<CsdlDocument> Handle()
        {
            return Task.FromResult(_MetadataCache.GetCsdlDocument(ForceUpdate));
        }
    }
}