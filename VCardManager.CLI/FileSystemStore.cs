using VCardManager.Core;

namespace VCardManager.CLI
{
    public class FileSystemStore : IFileStore
    {
        public bool Exist(string path) => File.Exists(path);
        public string ReadAllText(string path) => File.ReadAllText(path);
        public string[] ReadAllLines(string path) => File.ReadAllLines(path);
        public void WriteAllText(string path, string content) => File.WriteAllText(path, content);
        public void AppendAllText(string path, string content) => File.AppendAllText(path, content);
    }
}

/*
    #   `using VCardManager.Core;`  - It allows the `FileSystemStore` class, which resides in the namespace `VCardManager.CLI`, 
                                    to directly use types (in this case, IFileStore) from the namespace VCardManager.Core without 
                                    having to specify their fully qualified name (VCardManager.Core.IFileStore)
    
    #   `:`                         - This means that `FileSystemStore` commits to providing an implementation of all methods declared in `IFileStore`
                                    If any method from `IFileStore` was not implemented in `FileSystemStore`, the compiler would generate an error

    #   `=> File.Exists(path);`     - is an expression-bodied member. This is a shortened syntax for methods that consist of a single line of code
                                    It is equivalent to:
                                                        {
                                                            return File.Exists(path);
                                                        }
    
    #   `File.Exists(path)`         - is a static method from the System.IO.File class (which is part of the .NET Framework/.NET) 
                                    It checks if the file at the specified path exists on the file system. This is a real file system operation

    #   `Exists`                    - Gets the System.IO.FileAttributes of the file on the path

    #   `ReadAllText`               - Opens a file, reads all text in the file with the specified encoding, and then closes the file

    #   `WriteAllText`              - Creates a new file, writes the specified string to the file using the specified encoding, 
                                    and then closes the file. If the target file already exists, it is overwritten

    #   `AppendAllText`             - Appends the specified string to the file using the specified encoding, creating
                                    the file if it does not already exist
*/