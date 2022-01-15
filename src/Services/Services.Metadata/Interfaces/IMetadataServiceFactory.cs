namespace Rhyous.EntityAnywhere.Services
{
    public interface IMetadataServiceFactory
    {
        IMetadataService MetadataService { get;  }
        ICustomMetadataProvider CustomMetadataProvider { get; }
    }
}