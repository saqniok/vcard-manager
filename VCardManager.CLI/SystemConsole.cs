using VCardManager.Core;

namespace VCardManager.CLI;

public class SystemConsole : IConsole
{
    public void WriteLine(string message) => Console.WriteLine(message);        // This method outputs the specified string to the console and then moves the cursor to the new line
    public void Write(string message) => Console.Write(message);                // This method outputs the specified string to the console, BUT does not move the cursor to a new line,
                                                                                // allowing the following output to be displayed on the same line
    public string ReadLine() => Console.ReadLine() ?? string.Empty;             // Reads a single line of text entered by the user from the console (before pressing Enter).
                                                                                // It returns string, but if it's null `??` then returns string (string.Empty)
}

