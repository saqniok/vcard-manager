using System;
using VCardManager.Core;

namespace VCardManager.CLI
{
    public class Menu
    {
        private readonly IFacade _facade;
        private readonly IConsole _console;

        public Menu(IFacade facade, IConsole console)
        {
            _facade = facade;
            _console = console;
        }

        public void ShowOptions()
        {
            _console.WriteLine("\nMenu:");
            _console.WriteLine("1. Add Card");
            _console.WriteLine("2. Delete Card");
            _console.WriteLine("3. Export Card");
            _console.WriteLine("4. Find and Show Card");
            _console.WriteLine("5. Show All Cards");
            _console.WriteLine("0. Exit");
            _console.WriteLine("Add number in console: ");
        }

        public void Run()
        {
            bool exit = false;

            var actions = new Dictionary<string, Action>
            {
                ["1"] = () => _facade.AddVCard(),
                ["2"] = () => _facade.DeleteCard(),
                ["3"] = () => _facade.ExportCard(),
                ["4"] = () => _facade.FindByName(),
                ["5"] = () => _facade.ShowAllCards(),
                ["0"] = () => exit = true
            };

            while (!exit)
            {
                ShowOptions();
                var input = _console.ReadLine();

                if (actions.TryGetValue(input, out var action))
                    action.Invoke();
                else
                    _console.WriteLine("Wrong number, Try again and again and again....");
            }
        }
    }
}