namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IRootHandlerProvider
    {
        IEntitySettingsHandler EntitySettingsHandler { get; }
        IGenerateHandler GenerateHandler { get; }
        IGetMetadataHandler GetMetadataHandler { get; }
        ISeedEntityHandler SeedEntityHandler { get; }
    }

}