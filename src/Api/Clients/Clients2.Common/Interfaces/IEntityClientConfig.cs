namespace Rhyous.EntityAnywhere.Clients2
{
    public interface IEntityClientConfig
    {
        string EntityAdminToken { get; }
        string EntityWebHost { get; }
        string EntitySubpath { get; }
    }
}