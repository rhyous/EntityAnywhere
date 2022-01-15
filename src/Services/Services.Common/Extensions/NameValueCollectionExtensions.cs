using Rhyous.Collections;
using Rhyous.Odata.Filter;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Specialized;
using System.Linq.Expressions;

namespace Rhyous.EntityAnywhere.Services
{
    public static class NameValueCollectionExtensions
    {
        public static Expression<Func<T, bool>> GetFilterExpression<T>(this NameValueCollection parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return null;
            var filterString = parameters.Get("$filter", string.Empty);
            if (string.IsNullOrWhiteSpace(filterString))
                return null;
            var parser = new FilterExpressionParser<T>(FilterExpressionParserActionDictionary<T>.Instance);
            return parser.ParseAsync(filterString, true, null).Result;
        }

        public static string GetSortByProperty(this NameValueCollection parameters, Type entityType, string defaultSortByProperty = "Id")
        {
            if (parameters == null || parameters.Count == 0)
                return defaultSortByProperty;
            var sortString = parameters.Get("$orderBy", string.Empty);
            // If the query parameter is "$orderby=CreateDate asc" would split CreateDate and asc
            var split = sortString.Split();
            return string.IsNullOrWhiteSpace(split[0]) || entityType.GetProperty(split[0]) == null
                   ? defaultSortByProperty
                   : split[0];
        }

        public static SortOrder GetSortOrder(this NameValueCollection parameters, SortOrder defaultSortOrder)
        {
            if (parameters == null || parameters.Count == 0)
                return defaultSortOrder;
            var sortString = parameters.Get("$orderBy", string.Empty);
            if (sortString.EndsWith("desc", StringComparison.OrdinalIgnoreCase))
                return SortOrder.Descending;
            else if (sortString.EndsWith("asc", StringComparison.OrdinalIgnoreCase))
                return SortOrder.Ascending;
            else
                return defaultSortOrder;
        }
    }
}