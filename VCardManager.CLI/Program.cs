/*
    Этот проект содержит только точку входа приложения (Program.cs)
  и непроверяемые конкретные реализации интерфейсов, определенных
  в VCardManager.Core, такие SystemConsoleкак и 
  FileSystemStore(см. Ниже).
*/

using VCardManager.CLI;
using VCardManager.Core;

class Program
{
  static void Main(string[] args)
  {
    IFileStore fileStore = new FileSystemStore();
    ICardService cardService = new CardService(fileStore);
    IConsole console = new SystemConsole();
    IFacade facade = new Facade(cardService, console);

    var menu = new Menu(facade, console);
    menu.Run();
  }
}
