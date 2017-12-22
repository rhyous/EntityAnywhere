using System.Collections.Generic;
using System.Linq;

namespace Rhyous.BusinessRules
{
    public abstract class BusinessRuleCollectionBase : IBusinessRuleCollection
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public List<IBusinessRule> Rules
        {
            get { return _Rules ?? (_Rules = new List<IBusinessRule>()); }
            set { _Rules = value; }
        } private List<IBusinessRule> _Rules;

        public Dictionary<IBusinessRule, BusinessRuleResult> Results
        {
            get { return _Results ?? (_Results = new Dictionary<IBusinessRule, BusinessRuleResult>()); }
            set { _Results = value; }
        } private Dictionary<IBusinessRule, BusinessRuleResult> _Results;

        public virtual BusinessRuleResult IsMet()
        {
            foreach (var rule in Rules)
            {
                Results[rule] = rule.IsMet();
            }
            return new BusinessRuleResult { Result = Results.All(r => r.Value?.Result ?? false), FailedObjects = Results.Select(o => o as object).ToList() };
        }
    }
}
