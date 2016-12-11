namespace Rhyous.WebFramework.Interfaces
{
    public interface IAddendum : IId<long>, IAuditable
    {
        string Entity { get; set; }
        string EntityId { get; set; }
        string Property { get; set; }
        string Value { get; set; }
    }
}
