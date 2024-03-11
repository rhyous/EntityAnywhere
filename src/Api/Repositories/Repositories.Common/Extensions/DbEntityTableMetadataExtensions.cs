using System;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Rhyous.EntityAnywhere.Repositories
{
    /// <summary>
    /// This extension returns database table metadata (such as db table name) of the passed in entity type
    public static class DbEntityTableMetadataExtensions
    {
        /// <summary>
        ///  Gets the db table name of the passed in entity type
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="dbContext">The DbContext</param>
        /// <returns>Table name of the entity</returns>
        public static string GetTableName<TEntity>(this IBaseDbContext<TEntity> dbContext)
            where TEntity : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;

            string sql = objectContext.CreateObjectSet<TEntity>().ToTraceString();
            Regex regex = new Regex("FROM (?<table>.*) AS");
            Match match = regex.Match(sql);

            string tableName = match.Groups["table"].Value;
            return tableName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <remarks>From here: https://romiller.com/2015/08/05/ef6-1-get-mapping-between-properties-and-columns/</remarks>
        public static string GetColumnName<TEntity>(this IBaseDbContext<TEntity> context, string propertyName)
            where TEntity : class
        {
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;
            // Get the part of the model that contains info about the actual CLR types
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            // Get the entity type from the model that maps to the CLR type
            var type = typeof(TEntity);
            var entityType = metadata
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .Single(e => objectItemCollection.GetClrType(e) == type);
 
            // Get the entity set that uses this entity type
            var entitySet = metadata
                .GetItems<EntityContainer>(DataSpace.CSpace)
                .Single()
                .EntitySets
                .Single(s => s.ElementType.Name == entityType.Name);
 
            // Find the mapping between conceptual and storage model for this entity set
            var mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                    .Single()
                    .EntitySetMappings
                    .Single(s => s.EntitySet == entitySet);
 
            // Find the storage entity set (table) that the entity is mapped
            var tableEntitySet = mapping
                .EntityTypeMappings.Single()
                .Fragments.Single()
                .StoreEntitySet;
 
            // Return the table name from the storage entity set
            var tableName = tableEntitySet.MetadataProperties["Table"].Value ?? tableEntitySet.Name;
 
            // Find the storage property (column) that the property is mapped
            var columnName = mapping
                .EntityTypeMappings.Single()
                .Fragments.Single()
                .PropertyMappings
                .OfType<ScalarPropertyMapping>()
                .Single(m => m.Property.Name == propertyName)
                .Column
                .Name;
 
            return columnName;
        }

    }
}