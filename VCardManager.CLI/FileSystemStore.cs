using VCardManager.Core;

namespace VCardManager.CLI
{
    public class FileSystemStore : IFileStore
    {
        public bool Exist(string path) => File.Exists(path);
        public string ReadAllText(string path) => File.ReadAllText(path);
        public void WriteAllText(string path, string content) => File.WriteAllText(path, content);
        public void AppendAllText(string path, string content) => File.AppendAllText(path, content);
    }
}