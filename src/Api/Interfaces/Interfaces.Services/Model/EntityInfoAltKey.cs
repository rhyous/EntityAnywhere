using Rhyous.StringLibrary;
using System;
using System.Linq.Expressions;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class EntityInfoAltKey<TEntity, TAltKey> : EntityInfo<TEntity>, IEntityInfoAltKey<TEntity, TAltKey>
    {
        public string AlternateKeyProperty => _AlternateKeyProperty ?? (_AlternateKeyProperty = typeof(TEntity).GetAlternateKeyProperty());
        private string _AlternateKeyProperty;

        /// <inheritdoc />
        public Expression<Func<TEntity, TAltKey>> PropertyExpression => _PropertyExpression ?? (_PropertyExpression = AlternateKeyProperty.ToLambda<TEntity, TAltKey>());
        Expression<Func<TEntity, TAltKey>> _PropertyExpression;

        public Func<TEntity, TAltKey> PropertyExpressionMethod => _PropertyExpressionCompiled ?? (_PropertyExpressionCompiled = PropertyExpression.Compile());
        private Func<TEntity, TAltKey> _PropertyExpressionCompiled;
    }
}
