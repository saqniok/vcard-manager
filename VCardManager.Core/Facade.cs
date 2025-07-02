namespace VCardManager.Core
{

    public class Facade : IFacade                                   // TODO:
    {
        private readonly ICardService _cardService;
        private readonly IConsole _console;

        // Constructor

        // Facade (type param, type param)
        public Facade(ICardService cardService, IConsole console)       // Its job is to initialize a new Facade instance and set its initial state
        {
            _cardService = cardService;                                 // Assigns the value obtained via the `cardService` parameter to the `_cardService` private field
            _console = console;                                         // Similarly, assigns the passed IConsole instance to the _console private field
        }

        public VCard SelectCardByName(string prompt)
        {
            _console.Write(prompt);                                     // This is the message that will be shown to the user before requesting input
            var name = _console.ReadLine();                             // Calls the `ReadLine` method on the `_console` object, which reads a string of text entered by the user from the console
            var cards = _cardService.FindByName(name).ToList();         // Search for VCards containing the specified name and returns them as IEnumerable<VCard>

            /*
                .ToList(): 
                    LINQ extension method. It converts an IEnumerable<VCard> to a List<VCard>. 
                    This is necessary because List<T> provides properties such as Count (number of elements) and
                    the ability to access elements by index (cards[i]), which is useful for further logic
            */

            if (!cards.Any())                               // .Any() Determines whether any element of a sequence satisfies a condition. 
            {
                _console.WriteLine("Card not found. ");     
                return null;
            }
            
            // Not sure why, I gues there is another way to do
            if (cards.Count == 1)                           // .Count is a property of the List<VCard> object, which returns the number of items in the list
                return cards[0];                            // If we have only 1 card, it's return that array with 1 index

            _console.WriteLine("Find more Cards: ");        // if Count > 1 we will recieve message, that it found more Cards

            // For loop for writing cards with the same names to the console
            for (int i = 0; i < cards.Count; i++)
                _console.WriteLine($"{i + 1}. {cards[i].FullName}, TEL: {cards[i].PhoneNumber}, EMAIL: {cards[i].Email}, ID: {cards[i].Id}");

            // New user menu, to choose which card to select
            _console.Write("Add Card number: ");

            if (int.TryParse(_console.ReadLine(), out int index)            // This is a safe way to convert a string to an integer
                && index >= 1 && index <= cards.Count)                      // If the conversion is successful, the resulting integer is stored
            {                                                               // in a new variable `index`
                return cards[index - 1];
            }

            _console.WriteLine("Wrong number. ");                           // If the number entered is incorrect (not a number or out of range), an error message is displayed
            return null;
        }


        private void PrintCard(VCard card)
        {
            _console.WriteLine($"Имя: {card.FullName}");
            _console.WriteLine($"Телефон: {card.PhoneNumber}");
            _console.WriteLine($"Email: {card.Email}");
            _console.WriteLine($"Id: {card.Id}");
        }

        private bool isCardExist(VCard card)
        {
            return card != null;
        }

        public void AddVCard()
        {
            _console.WriteLine("Add name: ");
            var name = _console.ReadLine();

            _console.WriteLine("Add Phone Number: ");
            var phone = _console.ReadLine();

            _console.WriteLine("Add email: ");
            var email = _console.ReadLine();

            var card = new VCard                                    // A new instance of VCard class is created
            {
                FullName = name,                                    // The FullName property of the `card` object is assigned the value entered by the user and stored in the name variable
                PhoneNumber = phone,
                Email = email
            };
            _cardService.addCard(card);
            _console.WriteLine($"Card is added. Id: {card.Id}");    
        }

        public void DeleteCard()
        {
            var card = SelectCardByName("Enter name of card for deleting: ");

            if (!isCardExist(card))
                return;

            _console.WriteLine($"Delete this card '{card.FullName}' (Id: {card.Id})? (y/n)");
            if (_console.ReadLine()?.ToLower() == "y")
            {
                _cardService.deleteCard(card.Id);
                _console.WriteLine("Card is deleted. ");
            }
            else
            {
                _console.WriteLine("Canceled");
            }

        }

        public void ExportCard()
        {
            var card = SelectCardByName("Enter name of card for exporting: ");

            if (!isCardExist(card))
                return;

            _cardService.exportCard(card.Id);
            _console.WriteLine($"Card '{card.FullName}' exported. ");
        }

        public void FindByName()
        {
            var card = SelectCardByName("Enter card name for searching. ");

            if (!isCardExist(card))
                return;

            PrintCard(card);

        }

        public void ShowAllCards()
        {
            var cards = _cardService.getAllCards();

            if (!cards.Any())
            {
                _console.WriteLine("No cards. ");
                return;
            }

            foreach (var card in cards)
                _console.WriteLine($"- {card.FullName} (Id: {card.Id})");
        }

        public void ShowCard()
        {

        }
    }
}