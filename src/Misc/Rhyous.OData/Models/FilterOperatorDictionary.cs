using System;
using System.Collections.Generic;

namespace Rhyous.OData.Models
{
    public class FilterOperatorDictionary : Dictionary<string, LogicalOperator>
    {
        #region Singleton

        private static readonly Lazy<FilterOperatorDictionary> Lazy = new Lazy<FilterOperatorDictionary>(() => new FilterOperatorDictionary());
        public static FilterOperatorDictionary Instance => Lazy.Value;

        internal FilterOperatorDictionary() : base(StringComparer.OrdinalIgnoreCase)
        {
            Add("eq", LogicalOperator.Equals);
            Add("ne", LogicalOperator.NotEquals);
            Add("gt", LogicalOperator.GreaterThan);
            Add("ge", LogicalOperator.GreaterThanOrEqual);
            Add("lt", LogicalOperator.LessThan);
            Add("le", LogicalOperator.LessThanOrEqual);
            Add("and", LogicalOperator.And);
            Add("or", LogicalOperator.Or);
            Add("not", LogicalOperator.Not);
            Add("has", LogicalOperator.Has);
        }

        #endregion
    }
}
