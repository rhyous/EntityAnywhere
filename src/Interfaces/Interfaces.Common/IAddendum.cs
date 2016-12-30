namespace Rhyous.WebFramework.Interfaces
{
    public interface IAddendum : IEntity<long>, IAuditable
    {
        string Entity { get; set; }
        string EntityId { get; set; }
        string Property { get; set; }
        string Value { get; set; }
    }
}
