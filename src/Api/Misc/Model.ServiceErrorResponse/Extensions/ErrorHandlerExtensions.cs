using Rhyous.BusinessRules;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Models
{
    public static class ErrorHandlerExtensions
    {
        public static IEnumerable<KeyValuePair<string, ErrorResponseValues>> ConvertDictionaryToPair(this IDictionary dict)
        {
            var businessRuleResults = GetBusinessRuleResultsFromValues(dict.Values);            
            var values = GetErrorResponseValuesFromBusinessRuleResults(businessRuleResults);
            var keys = GetKeys(dict.Keys);
            return ConvertToKeyValuePairs(values.ToList(), keys.ToList());
        }

        private static IEnumerable<BusinessRuleResult> GetBusinessRuleResultsFromValues(ICollection errorResponseValues)
        {                      
            // Extract the value results into a Business Rule Result Object
            foreach (var val in errorResponseValues)
            {
                if (val is BusinessRuleResult)
                {
                   yield return (val as BusinessRuleResult);
                }
            }
        }

        private static IEnumerable<ErrorResponseValues> GetErrorResponseValuesFromBusinessRuleResults(IEnumerable<BusinessRuleResult> businessRuleResults)
        {
            // Populate the needed result values with an ErrorResponseValue Object
            foreach (var busRuleResult in businessRuleResults)
            {
                yield return (
                    new ErrorResponseValues
                    {
                        Type = busRuleResult.ToString(),
                        FailedObjects = busRuleResult.FailedObjects,
                        Result = busRuleResult.Result.ToString()
                    });
            }
        }

        private static IEnumerable<string> GetKeys(ICollection keys)
        {
            // Third: Get the Keys from the generic object
            foreach (var key in keys)
            {
                yield return key.ToString();
            }
        }

        private static IEnumerable<KeyValuePair<string, ErrorResponseValues>> ConvertToKeyValuePairs(IList<ErrorResponseValues> values, IList<string> keys)
        {
            // Populate the result with the extracted data.
            for (int i = 0; i < values.Count(); i++)
            {
                yield return new KeyValuePair<string, ErrorResponseValues>(keys[i], values[i]);
            }
        }
    }
}
