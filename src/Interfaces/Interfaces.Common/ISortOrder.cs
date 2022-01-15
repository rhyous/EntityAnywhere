namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// Use this for an entity that has an Order property.
    /// </summary>
    public interface ISortOrder // Named SortOrder to avoid confusing this with purchase order
    {
        int Order { get; set; }
    }
}
