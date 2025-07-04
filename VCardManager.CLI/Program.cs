/*
    Этот проект содержит только точку входа приложения (Program.cs)
  и непроверяемые конкретные реализации интерфейсов, определенных
  в VCardManager.Core, такие SystemConsoleкак и 
  FileSystemStore(см. Ниже).
*/

using VCardManager.CLI;
using VCardManager.Core;
using Microsoft.Extensions.DependencyInjection;


class Program
{
  static void Main(string[] args)
  {
    var serviceProvider = new ServiceCollection()               // AddSingleton: make 1 example for 1 life circle
        .AddSingleton<IFileStore, FileSystemStore>()
        .AddSingleton<CardProperty>()
        .AddSingleton<ICardService, CardService>()
        .AddSingleton<IConsole, SystemConsole>()
        .AddSingleton<IUserInteraction, UserInteraction>()
        .AddSingleton<IFacade, Facade>()
        .AddSingleton<Menu>()
        .BuildServiceProvider();

    using (serviceProvider)
    {
      try
      {
        var menu = serviceProvider.GetRequiredService<Menu>();
        menu.Run();
      }
      catch (Exception ex)
      {
        Console.WriteLine($"An error occurred: {ex.Message}");
        Console.WriteLine(ex.StackTrace);
      }
    }
  }
}

