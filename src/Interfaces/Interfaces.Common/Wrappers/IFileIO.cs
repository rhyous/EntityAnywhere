namespace Rhyous.Wrappers
{
    public interface IFileIO
    {
        bool Exists(string path);
        string ReadAllText(string path);
    }
}