namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface  IEntityConfiguration
    {
        string DefaultSortByProperty { get; set; }
        SortOrder DefaultSortOrder { get; set; }
    }

    public class EntityConfiguration : IEntityConfiguration
    {
        public string DefaultSortByProperty { get; set; } = "Id";
        public SortOrder DefaultSortOrder { get; set; }
    }
}
