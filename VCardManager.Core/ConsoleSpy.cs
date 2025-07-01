using VCardManager.Core;

namespace VCardManager.Tests;

public class ConsoleSpy : IConsole                                          // TODO:
{
    private readonly Queue<string> inputs = new();
    public List<string> Output = new();

    public void Write(string message) => Output.Add(message);
    public void WriteLine(string message) => Output.Add(message);
    public string ReadLine() => inputs.Count > 0 ? inputs.Dequeue() : string.Empty;
    public void AddInput(string input) => inputs.Enqueue(input);
}