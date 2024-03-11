using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Rhyous.Wrappers
{
    /// <summary>
    /// This wraps the static System.IO.File class
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FileIO : IFileIO
    {
        public bool Exists(string path) => File.Exists(path);
        public string ReadAllText(string path) => File.ReadAllText(path);
    }
}