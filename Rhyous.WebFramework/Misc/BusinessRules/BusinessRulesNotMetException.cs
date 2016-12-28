using System;
using System.Linq;

namespace Rhyous.BusinessRules
{
    public class BusinessRulesNotMetException : Exception
    {
        protected const string ErrorMessage = "Business Rules were not met. See the Data for the details.";

        public BusinessRulesNotMetException(IBusinessRuleCollection ruleCollection)
            : base(ErrorMessage)
        {
            GetFailedBusinessRules(ruleCollection);
        }

        public BusinessRulesNotMetException(string dataMesssage)
            : base(ErrorMessage)
        {
            Data.Add("Error", dataMesssage);
        }

        private void GetFailedBusinessRules(IBusinessRuleCollection ruleCollection)
        {
            if (ruleCollection == null) { return; }
            foreach (var ruleResult in ruleCollection.Results.Where(ruleResult => !ruleResult.Value.Result))
            {
                Data.Add(ruleResult.Key.Name, ruleResult.Value);
            }
        }
    }
}
