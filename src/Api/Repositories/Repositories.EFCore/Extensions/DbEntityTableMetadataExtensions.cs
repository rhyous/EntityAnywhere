using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
            var entityType = dbContext.Model.FindEntityType(typeof(TEntity))!;
            var schema = entityType.GetSchema();
            return entityType.GetTableName()!;
        }

        /// <summary>
        /// Gets the column name of a property.
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="dbContext">The DbContext</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The column name of a property.</returns>
        /// <remarks>From here: https://romiller.com/2015/08/05/ef6-1-get-mapping-between-properties-and-columns/</remarks>
        public static string GetColumnName<TEntity>(this IBaseDbContext<TEntity> dbContext, string propertyName)
            where TEntity : class
        {
            var tableName = GetTableName<TEntity>(dbContext);
            var entityType = dbContext.Model.FindEntityType(typeof(TEntity))!;
            var property = entityType.GetProperties().First(p => p.Name == propertyName);
            return property.GetColumnName(StoreObjectIdentifier.Table(tableName, null))!;
        }
    }
}