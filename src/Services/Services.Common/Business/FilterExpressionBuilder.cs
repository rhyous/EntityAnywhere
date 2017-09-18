using Rhyous.StringLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Services
{
    public class FilterExpressionBuilder<TEntity>
    {
        public FilterExpressionBuilder(string filterString)
        {
            _FilterString = filterString;
        }

        public string FilterString => _FilterString;
        private string _FilterString;

        public Expression<Func<TEntity, bool>> Expression { get { return _Expression ?? (_Expression = BuildExpression(FilterString)); } }
        private Expression<Func<TEntity, bool>> _Expression;
        
        public static Expression<Func<TEntity, bool>> BuildExpression(string filterExpression)
        {
            if (string.IsNullOrWhiteSpace(filterExpression))
                return null;
            var array = filterExpression.Split();
            if (array.Length != 3)
                return null;
            var property = array[0];
            var oper = Operators[array[1]];
            var type = typeof(TEntity).GetPropertyInfo(property).PropertyType;
            var value = array[2].ToType(type);
            return property.ToLambda<TEntity>(type, new[] { value, oper.Method, oper.Not });
        }

        internal static Dictionary<string, Operator> Operators = new Dictionary<string, Operator>
        {
            { "eq", new Operator {Method = "Equals", Not = false } },
            { "neq", new Operator {Method = "Equals", Not = true } },
        };

        internal class Operator
        {
            internal bool Not { get; set; }
            internal string Method { get; set; }
        }
    }
}
