namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IFileUpload
    {
        /// <summary>The name of the file, including the extension.</summary>
        string FileName { get; set; }

        /// <summary>The actual file in bytes.</summary>
        byte[] Data { get; set; }

        /// <summary>The file type.</summary>
        FileType FileType { get; set; }
    }
}