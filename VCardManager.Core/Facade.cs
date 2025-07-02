using System.Reflection.Metadata.Ecma335;

namespace VCardManager.Core
{

    public class Facade : IFacade                                   // TODO:
    {
        private readonly ICardService _cardService;
        private readonly IConsole _console;

        public Facade(ICardService cardService, IConsole console)
        {
            _cardService = cardService;
            _console = console;
        }

        public VCard SelectCardByName(string prompt)
        {
            _console.Write(prompt);
            var name = _console.ReadLine();
            var cards = _cardService.FindByName(name).ToList();

            if (!cards.Any())
            {
                _console.WriteLine("Card not found. ");
                return null;
            }

            if (cards.Count == 1)
            {
                return cards[0];
            }

            _console.WriteLine("Find more Cards: ");
            for (int i = 0; i < cards.Count; i++)
            {
                _console.WriteLine($"{i + 1}. {cards[i].FullName}");
            }

            _console.Write("Add Card number: ");
            if (int.TryParse(_console.ReadLine(), out int index)
                && index >= 1 && index <= cards.Count)
            {
                return cards[index - 1];
            }

            _console.WriteLine("Wrong number. ");
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

            var card = new VCard
            {
                FullName = name,
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