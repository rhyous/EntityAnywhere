using Rhyous.Collections;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class GenerateHandler : IGenerateHandler
    {
        private readonly IEntityCaller _EntityCaller;
        private readonly IEntityList _EntityList;
        private readonly IRequestUri _RequestUri;

        public GenerateHandler(IEntityCaller entityCaller, IEntityList entityList, IRequestUri requestUri)
        {
            _EntityCaller = entityCaller ?? throw new ArgumentNullException(nameof(entityCaller));
            _EntityList = entityList ?? throw new ArgumentNullException(nameof(entityList));
            _RequestUri = requestUri ?? throw new ArgumentNullException(nameof(requestUri));
        }

        public async Task<List<RepositoryGenerationResult>> Handle()
        {
            var rootUrl = _RequestUri.Uri.AbsoluteUri.Substring(0, _RequestUri.Uri.AbsoluteUri.IndexOf("Service/$Generate"));
            var results = (await _EntityCaller.CallAll<RepositoryGenerationResult>(rootUrl, "GenerateRepositoryAsync"))?.ToList();
            if (results == null)
                results = new List<RepositoryGenerationResult>();
            var notGeneratedTypes = _EntityList.Entities.Where(t => results.None(r => r.Name == t.Name));
            foreach (var type in notGeneratedTypes)
                results.Add(new RepositoryGenerationResult { Name = type.Name, FailureReason = "Unknown", RepositoryReady = false });
            return results.ToList();
        }
    }
}