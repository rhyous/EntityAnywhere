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
        public IQueryable<ICountry> Get(bool order = false, string orderBy = "Id")
        {
            var region = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID));
            return region.Select(r => new Country
            {
                Id = r.GeoId,
                Name = r.EnglishName,
                ThreeLetterIsoCode = r.ThreeLetterISORegionName,
                TwoLetterIsoCode = r.TwoLetterISORegionName
            }).Distinct(new CountryComparer()).OrderBy(c => c.Name).AsQueryable();
        }

        public IQueryable<ICountry> Get(Expression<Func<Country, int>> orderExpression)
        {
            var region = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID));
            return region.Select(r => new Country { Id = r.GeoId, Name = r.Name }).AsQueryable();
        }

        public IQueryable<ICountry> Get(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public ICountry Get(int id)
        {
            throw new NotImplementedException();
        }

        public ICountry Get(string name, System.Linq.Expressions.Expression<Func<Country, string>> propertyExpression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ICountry> GetByExpression(System.Linq.Expressions.Expression<Func<Country, bool>> expression, string orderBy = "Id")
        {
            throw new NotImplementedException();
        }

        public IQueryable<ICountry> GetByExpression(System.Linq.Expressions.Expression<Func<Country, bool>> expression, System.Linq.Expressions.Expression<Func<Country, int>> orderExpression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ICountry> Search(string searchString, params System.Linq.Expressions.Expression<Func<Country, string>>[] propertyExpressions)
        {
            throw new NotImplementedException();
        }

        #region Not implemented
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
