using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using Rhyous.Odata.Filter;
using Rhyous.StringLibrary;
using Rhyous.StringLibrary.Pluralization;
using Rhyous.EntityAnywhere.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities
{
    /// <summary>An IFilterConverter{TEntity} implementation that gets related entity data and uses that to convert the Filter{TEntity}.</summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class RelatedEntityExtensionFilterConverter<TEntity> : IRelatedEntityExtensionFilterConverter<TEntity>
    {
        private const string RelatedEntityExtensionType = "Extension";
        private readonly CsdlSchema _CsdlSchema;
        private readonly IRelatedEntityFilterDataProvider _FilterDataProvider;

        /// <summary>An IFilterConverter{TEntity} implementation that gets related entity data and uses that to convert the Filter{TEntity}.</summary>
        /// <param name="csdlSchema">The csdlSchema for all entities.</param>
        /// <param name="relatedEntityProvider">An implementation of IRelatedEntityProvider. This is not implemented for you.</param>
        public RelatedEntityExtensionFilterConverter(CsdlSchema csdlSchema,
                                                     IRelatedEntityFilterDataProvider filterDataProvider)
        {
            _CsdlSchema = csdlSchema;
            _FilterDataProvider = filterDataProvider;
        }

        /// <summary>
        /// Converts a Filter using Related Entity data using a dot syntax, where the Related Entity is first part and the property is the
        /// second part. So if you have an Organization entity with an Id and Name, and User entity with an OrganizationId property, you
        /// could Filter on User with the following: Organization.Name eq 'My Org'
        /// If the Organization "My Org" had an Id of 5027, then the filter would convert to this: OrganizationId eq 5027
        /// All the users for Organization 5027 "My Org" would be returned.
        /// </summary>
        /// <param name="filter">A Filter with a syntax designed to first get data from a RelatedEntity.</param>
        /// <returns>A filter for the current entity.</returns>
        /// <remarks>In this iteration, it must be a direct RelatedEntity with a local reference property, not a RelatedEntityForiegn or Mapping.</remarks>
        public bool CanConvert(Filter<TEntity> filter)
        {
            return (IsFilterValid(filter) && DotSyntaxIsValid(filter) && IsCsdlSchemaValid(filter));
        }

        /// <inheritdoc /> 
        public async Task<Filter<TEntity>> ConvertAsync(Filter<TEntity> filter)
        {
            if (!CanConvert(filter))
                return null;
            var entityName = typeof(TEntity).Name;
            var relatedExtensionEntityName = filter.Left.NonFilter.Split('.')[0];
            var property = filter.Left.NonFilter.Split('.')[1];

            var newFilter = filter.Clone(true, false);
            newFilter.Left = nameof(ExtensionEntity.Value);

            var urlParams = $"$Filter=Entity eq '{entityName}' and Property eq '{property}' and {newFilter}";

            var relatedEntities = await _FilterDataProvider.ProvideAsync(relatedExtensionEntityName, urlParams);
            if (relatedEntities == null || !relatedEntities.Any())
                return "1 eq 0"; // Nothing is found so just return a query that will return false.
            if (!_CsdlSchema.Entities.TryGetValue(typeof(TEntity).Name, out object objCsdl))
                return null;
            var idProperty = (objCsdl as CsdlEntity).Keys[0]; // Usually gets "Id"
            if (!(objCsdl as CsdlEntity).Properties.TryGetValue(idProperty, out object objCsdlIdProperty))
                return null;

            var localIdValues = relatedEntities.Select(re => re.Object.GetValue(nameof(ExtensionEntity.EntityId)).ToString()).Distinct().ToArray();

            var type = (objCsdlIdProperty as CsdlProperty).Type;
            if (type == CsdlConstants.EdmString)
            {
                localIdValues = localIdValues.Select(v => !v.IsQuoted() && v.Any(char.IsWhiteSpace) ? v.Replace("'", "''").Wrap("'") : v).ToArray();
                return new Filter<TEntity>
                {
                    Left = $"{idProperty}",
                    Method = "in",
                    Right = new ArrayFilter<TEntity, string> { Array = localIdValues }
                };
            }
            Filter<TEntity> convertedFilter = $"{idProperty} in ({string.Join(",", localIdValues)})";
            return convertedFilter;
        }

        /// <summary>Makes sure the filter is not null or incomplete or not a simple or array filter.</summary>
        /// <param name="filter">The Filter.</param>
        /// <returns>True if the Filter is valid for this converter. False otherwise.</returns>
        private static bool IsFilterValid(Filter<TEntity> filter)
        {
            return filter != null
                && filter.IsComplete
                && !filter.IsSimpleString
                && !filter.IsArray
                && filter.Left != null
                && filter.Left.IsSimpleString;
        }

        /// <summary>Makes sure the synax is correct: {ExtensionEntity}.{ExtensionEntityProperty}</summary>
        /// <param name="filter">The Filter.</param>
        /// <returns>True if the dot syntax is correct, false otherwise.</returns>
        private static bool DotSyntaxIsValid(Filter<TEntity> filter)
        {
            return filter.Left.NonFilter.Count(s => s == '.') == 1
            && filter.Left.NonFilter.IndexOf('.') != 0 // The . is not the first character
            && filter.Left.NonFilter.IndexOf('.') != filter.Left.NonFilter.Length - 1; // The . is not the last character
        }

        /// <summary>Makes sure the Schema is valid and that the schema is for an Extension entity.</summary>
        /// <param name="filter">The Filter.</param>
        /// <returns>True if the schema is valid and is for an extension entity. False otherwise.</returns>
        private bool IsCsdlSchemaValid(Filter<TEntity> filter)
        {
            var extensionEntityName = filter.Left.NonFilter.Split('.')[0];
            return _CsdlSchema.Entities.TryGetValue(typeof(TEntity).Name, out object objCsdl)
                && (objCsdl as CsdlEntity) != null
                && (objCsdl as CsdlEntity).Properties.TryGetValue(extensionEntityName.Pluralize(), out object objCsdlProperty)
                && (objCsdlProperty as CsdlNavigationProperty) != null
                && (objCsdlProperty as CsdlNavigationProperty).Kind == CsdlConstants.NavigationProperty
                && (objCsdlProperty as CsdlNavigationProperty).CustomData.TryGetValue(CsdlConstants.EAFRelatedEntityType, out object value)
                && value.ToString() == RelatedEntityExtensionType;
        }
    }
}