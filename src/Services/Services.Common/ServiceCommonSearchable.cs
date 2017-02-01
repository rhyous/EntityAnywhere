﻿using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Services
{
    public class ServiceCommonSearchable<T, Tinterface, Tid> : ServiceCommon<T, Tinterface, Tid>, ISearchableServiceCommon<T, Tinterface, Tid>
        where T : class, Tinterface, new()
        where Tinterface : IEntity<Tid>
        where Tid : IComparable, IConvertible, IComparable<Tid>, IEquatable<Tid>
    {
        public ServiceCommonSearchable()
        {
        }

        public ServiceCommonSearchable(Expression<Func<T, string>> propertyExpression)
        {
            PropertyExpression = propertyExpression;
        }

        public virtual Expression<Func<T, string>> PropertyExpression { get; private set; }

        public Tinterface Get(string stringProperty)
        {
            return Repo.Get(stringProperty, PropertyExpression);
        }

        public List<Tinterface> Search(string stringProperty)
        {
            return Repo.Search(stringProperty, PropertyExpression);
        }

        public override List<Tinterface> Add(IList<Tinterface> entities)
        {
            var duplicates = new List<string>();
            foreach (var entity in entities)
            {
                var method = PropertyExpression.Compile();
                var text = method(entity as T);
                if (Get(text) != null)
                    duplicates.Add(text);
            }
            if (duplicates.Count > 0)
                throw new Exception($"Duplicate {typeof(T).Name}(s) detected: {(string.Join(", ", duplicates))}");
            return base.Add(entities);
        }
    }
}