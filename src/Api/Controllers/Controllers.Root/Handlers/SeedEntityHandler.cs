using Rhyous.Collections;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class SeedEntityHandler : ISeedEntityHandler
    {
        private readonly IEntityCaller _EntityCaller;
        private readonly IEntityList _EntityList;
        private readonly IRequestUri _RequestUri;

        public SeedEntityHandler(IEntityCaller entityCaller, 
                                 IEntityList entityList, 
                                 IRequestUri requestUri)
        {
            _EntityCaller = entityCaller ?? throw new ArgumentNullException(nameof(entityCaller));
            _EntityList = entityList ?? throw new ArgumentNullException(nameof(entityList));
            _RequestUri = requestUri ?? throw new ArgumentNullException(nameof(requestUri));
        }

        public async Task<List<RepositorySeedResult>> Handle()
        {
            var rootUrl = _RequestUri.Uri.AbsoluteUri.Substring(0, _RequestUri.Uri.AbsoluteUri.IndexOf("Service/$Seed"));
            var results = (await _EntityCaller.CallAll<RepositorySeedResult>(rootUrl, "InsertSeedDataAsync"))?.ToList();
            var notGeneratedTypes = _EntityList.Entities.Where(t => results.None(r => r.Name == t.Name));
            foreach (var type in notGeneratedTypes)
                results.Add(new RepositorySeedResult { Name = type.Name, FailureReason = "Unknown", SeedSuccessful = false });
            return results.ToList();
        }
    }
}