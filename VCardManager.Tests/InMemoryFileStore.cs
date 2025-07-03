namespace VCardManager.Tests
{
    using VCardManager.Core;
    using System.Collections.Generic;

    public class InMemoryFileStore : IFileStore
    {
        private readonly Dictionary<string, string> _store = new();                   // It's used to store “files” <key, value>

        public bool Exist(string path) => _store.ContainsKey(path);
        // `_store.ContainsKey(path)` 
        //  A standard Dictionary method that returns `true` if the dictionary contains the key corresponding to path

        public string ReadAllText(string path) => _store.TryGetValue(path, out var content) ? content : "";
        // This method “writes” all content to a “file” at the specified path
        // This is a safe way to get the `value` from the dictionary. 
        // It tries to find the `path` `key`. If the `key` is found, 
        // `true` is returned and the content is stored in the content variable

        // If .TryGetValue() returns false, returns an empty string (“”), mimicking the behavior of File.ReadAllText

        public void WriteAllText(string path, string contents) => _store[path] = contents;
        // If the path key already exists, its value is overwritten with the new contents. 
        // If the key does not exist, it is created and contents is assigned to it

        public void AppendAllText(string path, string contents)
        {
            if (_store.ContainsKey(path)) _store[path] += contents;     // If the “file” already exists, the new contents are concatenated to the current contents
            else _store[path] = contents;                               // If the “file” does not exist, it is “created” with the contents as its initial content
        }

        public string[] ReadAllLines(string path) => _store.TryGetValue(path, out var content) ? content.Split(["\r\n", "\n"], StringSplitOptions.None) : [];
    }
}

/*
    `InMemoryFileStore` is a fake implementation of the `IFileStore` interface. 
    Instead of working with a real file system (like FileSystemStore), 
    it simulates file operations using a regular `Dictionary<string, string>` in memory
*/