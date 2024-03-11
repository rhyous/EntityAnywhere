using Rhyous.StringLibrary;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class EntityInfoMapping<TEntity, TE1Id, TE2Id> 
        : EntityInfo<TEntity>, IEntityInfoMapping<TEntity, TE1Id, TE2Id>
        where TE1Id : IComparable, IComparable<TE1Id>, IEquatable<TE1Id>
        where TE2Id : IComparable, IComparable<TE2Id>, IEquatable<TE2Id>
    {
        public PropertyInfo E1PropertyInfo => _E1PropertyInfo ?? (_E1PropertyInfo = typeof(TEntity).GetMappedEntity1PropertyInfo());
        private PropertyInfo _E1PropertyInfo;

        public PropertyInfo E2PropertyInfo => _E2PropertyInfo ?? (_E2PropertyInfo = typeof(TEntity).GetMappedEntity2PropertyInfo());
        private PropertyInfo _E2PropertyInfo;

        /// <inheritdoc />
        public Expression<Func<TEntity, TE1Id>> E1PropertyExpression => _E1PropertyExpression ?? (_E1PropertyExpression = E1PropertyInfo.Name.ToLambda<TEntity, TE1Id>());
        Expression<Func<TEntity, TE1Id>> _E1PropertyExpression;

        public Func<TEntity, TE1Id> E1PropertyExpressionMethod => _E1PropertyExpressionCompiled ?? (_E1PropertyExpressionCompiled = E1PropertyExpression.Compile());
        private Func<TEntity, TE1Id> _E1PropertyExpressionCompiled;


        public Expression<Func<TEntity, TE2Id>> E2PropertyExpression => _E2PropertyExpression ?? (_E2PropertyExpression = E2PropertyInfo.Name.ToLambda<TEntity, TE2Id>());
        Expression<Func<TEntity, TE2Id>> _E2PropertyExpression;

        public Func<TEntity, TE2Id> E2PropertyExpressionMethod => _E2PropertyExpressionCompiled ?? (_E2PropertyExpressionCompiled = E2PropertyExpression.Compile());
        private Func<TEntity, TE2Id> _E2PropertyExpressionCompiled;
    }
}
