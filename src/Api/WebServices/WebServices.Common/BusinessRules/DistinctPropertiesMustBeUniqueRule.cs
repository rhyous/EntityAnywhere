using LinqKit;
using Rhyous.BusinessRules;
using Rhyous.Collections;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class DistinctPropertiesMustBeUniqueRule<TEntity, TInterface, TId> : IBusinessRule
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        internal EntityClientAsync Client;
        internal IEnumerable<string> RelatedEntityIds;
        private readonly ChangeType _ChangeType;

        public DistinctPropertiesMustBeUniqueRule(IEnumerable<TEntity> entities, 
                                                  IEnumerable<PropertyInfo> propertyInfos,
                                                  IServiceCommon<TEntity, TInterface, TId> service,
                                                  ChangeType changeType)
        {
            Entities = entities;
            PropertyInfos = propertyInfos;
            Service = service;
            _ChangeType = changeType;
        }

        public string Name => $"Distinct{Client?.Entity}MustBeUniqueRule:";

        public string Description => $"The distinct properties for {Client?.Entity} must be unique.";

        public BusinessRuleResult IsMet()
        {
            var attribs = PropertyInfos.SelectMany(pi => pi.GetCustomAttributes<DistinctPropertyAttribute>(true));
            if (attribs == null || !attribs.Any())
                return new BusinessRuleResult { Result = true };
            var groups = attribs.GroupBy(a => a.Group);

            var fullExpression = PredicateBuilder.New<TEntity>(false);
            var duplicates = new List<TEntity>();
            var expressions = new HashSet<string>();
            foreach (var group in groups)
            {
                foreach (var entity in Entities)
                {
                    var subExpression = PredicateBuilder.New<TEntity>(true);                    
                    // Build subExpression
                    foreach (var dpa in group)
                    {
                        var value = entity.GetPropertyValue(dpa.Property);
                        var equalsExpression = dpa.Property.ToLambda<TEntity>(value.GetType(), value, "eq");
                        subExpression.And(equalsExpression);
                    }

                    // Check for duplicates in the POSTed data using subExpression
                    // Big O of 1 check by using HashSet
                    var expressionString = subExpression.ToString();
                    if (expressions.Contains(expressionString))
                    {
                        duplicates.Add(entity);
                        continue;
                    }
                    expressions.Add(expressionString);

                    // Add subExpresion to fullExpression
                    fullExpression.Or(subExpression);
                }
            }
            if (_ChangeType == ChangeType.Update)
            {
                var ids = Entities.Select(e => e.Id).ToList();
                fullExpression.And(e => !ids.Contains(e.Id));
            }

            var results = Service.Get(fullExpression);

            if (results.Any())
            {
                duplicates.AddRange(results.ToConcrete<TEntity, TInterface>());
            }

            if (duplicates.Any())
            {
                return new BusinessRuleResult { Result = false, FailedObjects = duplicates.Select(r => r as object).ToList() };
            }
            return new BusinessRuleResult { Result = true };
        }

        public IServiceCommon<TEntity, TInterface, TId> Service { get; }
        public IEnumerable<PropertyInfo> PropertyInfos { get; }
        public IEnumerable<TEntity> Entities { get; }
    }
}