using Rhyous.Odata;
using Rhyous.Odata.Filter;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities
{
    class CustomFilterConverterCollection<TEntity> : ICustomFilterConverterCollection<TEntity>
    {
        public CustomFilterConverterCollection(RelatedEntityFilterConverter<TEntity> relatedEntityFilterConverter)
        {
            Converters.Add(relatedEntityFilterConverter);
        }

        public List<IFilterConverter<TEntity>> Converters
        {
            get { return _Converters ?? (_Converters = new List<IFilterConverter<TEntity>>()); }
            internal set { _Converters = value; }
        } private List<IFilterConverter<TEntity>> _Converters;

    }
}