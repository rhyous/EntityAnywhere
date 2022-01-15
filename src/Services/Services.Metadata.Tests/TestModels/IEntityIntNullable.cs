namespace Rhyous.EntityAnywhere.Services.Tests
{
    public interface IEntityIntNullable
    {
        int Id { get; set; }
        string Name { get; set; }
        int? OptionalId { get; set; }
    }
}