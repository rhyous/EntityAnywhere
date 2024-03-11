using Newtonsoft.Json;
using Rhyous.BusinessRules;
using Rhyous.Collections;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class RelatedEntityMustExistRule : IBusinessRule
    {
        internal IEntityClientAsync _Client;
        internal IEnumerable<string> _RelatedEntityIds;
        private readonly object _AllowedNonExistentValue;
        private readonly bool _Nullable;

        public RelatedEntityMustExistRule(IEntityClientAsync entityClient, 
                                          IEnumerable<string> relatedEntityIds,
                                          object allowedNonExistentValue,
                                          bool nullable)
        {
            _Client = entityClient ?? throw new ArgumentNullException(nameof(entityClient));
            _RelatedEntityIds = relatedEntityIds?.Distinct().ToList() ?? throw new ArgumentNullException(nameof(relatedEntityIds));
            _AllowedNonExistentValue = allowedNonExistentValue;
            _Nullable = nullable;
        }

        public string Name => $"Related{_Client?.Entity}MustExistRule:";

        public string Description => $"The related {_Client?.Entity} must exist.";

        public BusinessRuleResult IsMet() => TaskRunner.RunSynchonously(IsMetAsync);

        internal async Task<BusinessRuleResult> IsMetAsync()
        {
            if (_Nullable)
            {
                _RelatedEntityIds = _RelatedEntityIds.Where(e => e != null);
                if (!_RelatedEntityIds.Any())
                    return new BusinessRuleResult { Result = true };
            }
            else
            {
                var nulls = _RelatedEntityIds.Where(e => e == null);
                if (nulls.Any())
                    return new BusinessRuleResult { Result = false, FailedObjects = nulls.ToList<object>() };
            }
            if (_AllowedNonExistentValue != null)
            {
                _RelatedEntityIds = _RelatedEntityIds.Where(e => e != _AllowedNonExistentValue.ToString());
                if (!_RelatedEntityIds.Any())
                    return new BusinessRuleResult { Result = true };
            }
            var json = await _Client.GetByIdsAsync(_RelatedEntityIds, true);
            var collection = JsonConvert.DeserializeObject<OdataObjectCollection>(json);
            var ids = collection.Select(e => e.Id).ToList();
            var mismatches = _RelatedEntityIds.GetMismatchedItems(ids);
            var result = new BusinessRuleResult { Result = !mismatches.Any() };
            if (!result.Result)
                result.FailedObjects.AddRange(mismatches.Left);
            return result;
        }
    }
}