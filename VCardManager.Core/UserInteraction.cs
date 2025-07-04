using System.Text.RegularExpressions;

namespace VCardManager.Core
{
    /**
    Intercace
    */
    public interface IUserInteraction
    {
        bool ConfirmDelete(VCard card);
        VCard GetContactInfoFromUser();

        string GetUserInput(string prompt);
    }

    /**
    Calss
    */
    public class UserInteraction : IUserInteraction
    {
        private readonly IConsole _console;

        // Constructor
        public UserInteraction(IConsole console)
        {
            _console = console;
        }

        // Methods
        public VCard GetContactInfoFromUser()
        {
            string name = GetUserInput("Add name: ");
            string phone;
            while (true)
            {
                phone = GetUserInput("Add Phone Number: ");

                if (IsValidPhoneNumber(phone))
                    break;

                Console.WriteLine("Invalid phone number format. Please try again.");
            }

            string email = GetUserInput("Add email: ");

            var card = new VCard                                    // A new instance of VCard class is created
            {
                FullName = name,                                    // The FullName property of the `card` object is assigned the value entered by the user and stored in the name variable
                PhoneNumber = phone,
                Email = email
            };
            return card;
        }

        public string GetUserInput(string prompt)
        {
            _console.WriteLine(prompt);
            var name = _console.ReadLine();
            return name;
        }

        public bool ConfirmDelete(VCard card)
        {
            _console.WriteLine($"Delete this card '{card.FullName}' (Id: {card.Id})? (y/n)");
            var input = _console.ReadLine();
            var userConfirmed = string.Equals(input, "y", StringComparison.InvariantCultureIgnoreCase);
            if (userConfirmed)
            {
                _console.WriteLine("Card is deleted. ");
            }
            else
            {
                _console.WriteLine("Canceled");
            }
            return userConfirmed;
        }

        public static bool IsValidPhoneNumber(string input)
        {
            var pattern = @"^0?\d{3,4}([/\. ]?\d{2}){3}$";
            return Regex.IsMatch(input, pattern);
            /**
                    @"^0?487([/\. ]?\d{2}){3}$" explanation

                    `@`     begore " verbatim string literal, it's convert double \\ to \, C# interprets all characters within a string literally

                    `^`     Begin of the string. This means that the pattern must match the very beginning of the input string

                    0?      Means that `0` is optional. May or may not be

                    \d{3,4} Means any 3 or 4 numbers

                    ([/\. ]?\d{2})   Optional sign can be added like /\. and " ", after that must be 2 digits

                    {3}     Means 3 times, what is in scopes

                    $       End ofthe regEx
            */
        }
    }
}