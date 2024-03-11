using Rhyous.Odata.Filter;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities
{
    class CustomFilterConverterCollection<TEntity> : ICustomFilterConverterCollection<TEntity>
    {
        public CustomFilterConverterCollection(IRelatedEntityFilterConverter<TEntity> relatedEntityFilterConverter,
                                               IRelatedEntityExtensionFilterConverter<TEntity> relatedEntityExtensionFilterConverter)
        {
            Converters = new List<IFilterConverter<TEntity>>
            {
                relatedEntityFilterConverter,
                relatedEntityExtensionFilterConverter
            };
        }

        public List<IFilterConverter<TEntity>> Converters { get;set; }

    }
}