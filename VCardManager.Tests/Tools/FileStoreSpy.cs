using VCardManager.Core;

namespace VCardManager.Tests._tools;

public class FileStoreSpy : IFileStore
{
    public bool FileExists { get; set; } = true;
    public string Text { get; set; } = string.Empty;
    public string LastPath { get; set; } = string.Empty;
    private FileStoreSpy SetPath(string path) { LastPath = path; return this; }
    public bool Exist(string path) => SetPath(path).FileExists;
    public string ReadAllText(string path) => SetPath(path).Text;
    public void WriteAllText(string path, string contents) => SetPath(path).Text = contents;
    public void AppendAllText(string path, string contents) => SetPath(path).Text += contents;

    public string[] ReadAllLines(string path) => File.ReadAllLines(path);
}