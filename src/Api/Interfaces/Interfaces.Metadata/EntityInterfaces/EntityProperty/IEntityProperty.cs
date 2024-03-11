namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityProperty 
        : IBaseEntity<int>,
          IName, 
          IDescription, 
          ISortOrder, 
          IAuditable
    {
        int EntityId { get; set; }
        bool Searchable { get; set; }
        bool Nullable { get; set; }
        bool VisibleOnRelations { get; set; }
        string Type { get; set; }
    }
}
