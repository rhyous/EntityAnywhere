using Rhyous.StringLibrary;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

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

        internal ICountry GetCountry(int id)
        {
            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID));
            var region = regions.FirstOrDefault(r => r.GeoId == id);
            return new Country
            {
                Id = region.GeoId,
                Name = region.EnglishName,
                ThreeLetterIsoCode = region.ThreeLetterISORegionName,
                TwoLetterIsoCode = region.TwoLetterISORegionName
            };
        }

        public IQueryable<ICountry> Get(bool order = false, string orderBy = "Id")
        {
            return GetCountries().AsQueryable().OrderBy(c => c.Name);
        }

        public IQueryable<ICountry> Get(Expression<Func<Country, int>> orderExpression)
        {
            return GetCountries().AsQueryable().OrderBy(orderExpression);
        }

        public IQueryable<ICountry> Get(List<int> ids)
        {
            return GetCountries().Where(c => ids.Contains(c.Id)).AsQueryable();
        }

        public ICountry Get(int id)
        {
            return GetCountry(id);
        }

        public ICountry Get(string name, Expression<Func<Country, string>> propertyExpression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ICountry> GetByExpression(Expression<Func<Country, bool>> expression, string orderBy = "Id")
        {
            return GetCountries().AsQueryable().Where(expression).OrderBy(orderBy.ToLambda<Country, int>());
        }

        public IQueryable<ICountry> GetByExpression(Expression<Func<Country, bool>> expression, Expression<Func<Country, int>> orderExpression)
        {
            return GetCountries().AsQueryable().Where(expression).OrderBy(orderExpression);
        }

        #region Not implemented
        public IQueryable<ICountry> Search(string searchString, params Expression<Func<Country, string>>[] propertyExpressions)
        {
            throw new NotImplementedException();
        }

        public List<ICountry> Create(IList<ICountry> items)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ICountry Update(ICountry item, IEnumerable<string> changedProperties)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
