using Rhyous.StringLibrary;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Interfaces.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Rhyous.WebFramework.Repositories
{
    public class CountryRepository : IRepository<Country, ICountry, int>
    {
        internal IEnumerable<Country> GetCountries()
        {
            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID));
            return regions.Select(r => new Country
            {
                Id = r.GeoId,
                Name = r.EnglishName,
                ThreeLetterIsoCode = r.ThreeLetterISORegionName,
                TwoLetterIsoCode = r.TwoLetterISORegionName
            }).Distinct(new CountryComparer());
        }

        public virtual ICountry Get(int id)
        {
            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID));
            var region = regions.FirstOrDefault(r => r.GeoId == id);
            if (region == null)
                return null;
            return new Country
            {
                Id = region.GeoId,
                Name = region.EnglishName,
                ThreeLetterIsoCode = region.ThreeLetterISORegionName,
                TwoLetterIsoCode = region.TwoLetterISORegionName
            };
        }

        public virtual IQueryable<ICountry> Get(string orderBy = "Name", SortOrder sortOrder = SortOrder.Ascending)
        {
            if (orderBy == nameof(Country.Id))
                return Get<int>(orderBy, sortOrder);
            if (orderBy == nameof(Country.Name) || orderBy == nameof(Country.ThreeLetterIsoCode) || orderBy == nameof(Country.TwoLetterIsoCode))
                return Get<string> (orderBy, sortOrder);

            // The below code is almost unreachable, unless additional properties are added to the Country entity.
            var propInfo = typeof(Country).GetProperty(orderBy);
            if (propInfo == null) { throw new ArgumentException($"The parameter must be a property of {typeof(Country).Name}", nameof(orderBy)); }

            // Build the forward-to-method dynamically using reflection
            var getMethodInfo = GetType().GetMethods()
                                            .FirstOrDefault(m => m.Name == nameof(Get)
                                                              && m.GetGenericArguments().Length == 1
                                                              && m.GetGenericArguments().First().Name == "TProperty"
                                                              && m.GetParameters().Length == 2);
            var getMethod = getMethodInfo.MakeGenericMethod(propInfo.PropertyType);
            try { return getMethod.Invoke(this, new object[] { orderBy, sortOrder }) as IQueryable<ICountry>; }
            catch (Exception e) { return ExceptionUnwrapper.HandleException<TargetInvocationException, IQueryable<ICountry>>(e); }
        }

        public virtual IQueryable<ICountry> Get<TProperty>(string orderBy = "Name", SortOrder sortOrder = SortOrder.Ascending)
        {
            var query = GetCountries().AsQueryable().OrderBy(c => c.Name).AsQueryable();
            if (string.IsNullOrWhiteSpace(orderBy)) // Even with the default parameter, this is needed for empty or whitespace.
                orderBy = "Name";
            return sortOrder == SortOrder.Ascending
                 ? query.OrderBy(orderBy.ToLambda<Country, TProperty>())
                 : query.OrderByDescending(orderBy.ToLambda<Country, TProperty>());
        }
        public virtual IQueryable<ICountry> Get(Expression<Func<Country, int>> orderExpression)
            => Get<int>(orderExpression);

        public virtual IQueryable<ICountry> Get<TProperty>(Expression<Func<Country, TProperty>> orderExpression)
        {
            return GetCountries().AsQueryable().OrderBy(orderExpression);
        }

        public virtual IQueryable<ICountry> Get(IEnumerable<int> ids)
        {
            return GetCountries().Where(c => ids.Contains(c.Id)).AsQueryable();
        }

        public virtual IQueryable<ICountry> GetByExpression(Expression<Func<Country, bool>> expression, 
                                                    string orderBy = "Id", 
                                                    SortOrder sortOrder = SortOrder.Ascending)
        {
            return GetCountries().AsQueryable().Where(expression).OrderBy(orderBy.ToLambda<Country, int>());
        }

        #region Not implemented

        public virtual ICountry Get<TResult>(TResult propertyValue, Expression<Func<Country, TResult>> propertyExpression)
            where TResult : IComparable, IComparable<TResult>, IEquatable<TResult>
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<ICountry> Search<TResult>(TResult searchValue, params Expression<Func<Country, TResult>>[] propertyExpressions)
        {
            throw new NotImplementedException();
        }

        public virtual List<ICountry> Create(IEnumerable<ICountry> items)
        {
            throw new NotImplementedException();
        }

        public virtual bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public virtual Dictionary<int,bool> DeleteMany(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public virtual ICountry Update(PatchedEntity<ICountry, int> patchedEntity, bool stage = false)
        {
            throw new NotImplementedException();
        }

        public virtual List<ICountry> BulkUpdate(PatchedEntityCollection<ICountry, int> patchedEntityCollection, bool stage = false)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<ICountry> GetByExpression<TProperty>(Expression<Func<Country, bool>> expression, 
                                                               Expression<Func<Country, TProperty>> orderExpression,
                                                               SortOrder sortOrder)
        {
            throw new NotImplementedException();
        }

        public virtual RepositoryGenerationResult GenerateRepository()
        {
            return new RepositoryGenerationResult()
            {
                Name = "File",
                FailureReason = "No generation necessary.",
                RepositoryReady = true
            };
        }

        #endregion

        #region IDisposable

        public virtual void Dispose() { } // Nothing to do

        #endregion

    }
}
