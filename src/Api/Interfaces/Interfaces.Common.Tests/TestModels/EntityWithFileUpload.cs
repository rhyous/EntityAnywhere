namespace Rhyous.EntityAnywhere.Interfaces.Common.Tests
{
    public interface IEntityWithFileUpload : IFileUpload { }

    public class EntityWithFileUpload : IEntityWithFileUpload
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
        public FileType FileType { get; set; }
    }
}
