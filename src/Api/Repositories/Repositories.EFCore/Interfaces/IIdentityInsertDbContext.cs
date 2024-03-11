namespace Rhyous.EntityAnywhere.Repositories
{
    /// <summary>
    /// DbContext for identity insert
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TId">The type of the entity's Id field.</typeparam>
    public interface IIdentityInsertDbContext<TEntity, TId> : IBaseDbContext<TEntity>
        where TEntity : class
    {
    }
}
