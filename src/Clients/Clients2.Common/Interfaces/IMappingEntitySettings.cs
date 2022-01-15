namespace Rhyous.EntityAnywhere.Clients2
{
    public partial interface IMappingEntitySettings
    {
        string Entity1 { get; }
        string Entity1Pluralized { get; }
        string Entity1Property { get; }
        string Entity2 { get; }
        string Entity2Pluralized { get; }
        string Entity2Property { get; }
    }

    public partial interface IMappingEntitySettings<TEntity> : IMappingEntitySettings
    {
    }
}