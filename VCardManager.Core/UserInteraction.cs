namespace VCardManager.Core
{
    /**
    Intercace
    */
    public interface IUserInteraction
    {
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
            string phone = GetUserInput("Add Phone Number: ");
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
    }
}