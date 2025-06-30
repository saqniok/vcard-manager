namespace VCardManager.Tests
{
    using VCardManager.Core;
    using System.Collections.Generic;

    public class InMemoryFileStore : IFileStore
    {
        private readonly Dictionary<string, string> _store = new();

        public bool Exist(string path) => _store.ContainsKey(path);
        public string ReadAllText(string path) => _store.TryGetValue(path, out var content) ? content : "";
        public void WriteAllText(string path, string contents) => _store[path] = contents;
        public void AppendAllText(string path, string contents)
        {
            if (_store.ContainsKey(path)) _store[path] += contents;
            else _store[path] = contents;
        }
    }
}