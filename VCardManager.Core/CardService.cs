namespace VCardManager.Core
{
    public class CardService : ICardService                                             // Since we inherit the interface, we have to declare each method
    {
        private readonly string _filePath = "data/contacts.vcf";                        // `readonly` means that the value of this field can be set only when declaring it or
                                                                                        //  in the class constructor. It cannot be changed after that. 
                                                                                        //  This ensures that the path to the file remains constant

        private readonly IFileStore _fileStore;
        private readonly CardProperty _cardProperty;

        public CardService(IFileStore fileStore, CardProperty cardProperty)
        {
            _fileStore = fileStore ?? throw new ArgumentNullException(nameof(fileStore));
            _cardProperty = cardProperty ?? throw new ArgumentNullException(nameof(cardProperty));
        }

        public void addCard(VCard card)                                                 // This method adds a new VCard to the repository
        {
            if (card == null) throw new ArgumentNullException(nameof(card));            // `ArgumentNullException`, which prevents potential errors and indicates incorrect use of
                                                                                        //  the method. nameof(card) returns the name of the parameter as a string

            _fileStore.AppendAllText(_filePath, card.ToString() + Environment.NewLine);         // is used to add text to the end of the file specified in _filePath
                                                                                                // `Environment.NewLine` adds a line break so that each VCard starts on a new line in the file
        }

        public void deleteCard(Guid id)
        {
            var cards = getAllCards().ToList();                                         // First, it gets all the VCards from the file using the getAllCards() method
                                                                                        // .ToList() converts an IEnumerable<VCard> to a List<VCard>, which allows 
                                                                                        // to modify the collection (remove elements)

            var cardToRemove = cards.FirstOrDefault(c => c.Id == id);                   // LINQ method `FirstOrDefault()` to find the first VCard in the list
                                                                                        // that has an Id matching the given id. If there is no such card, cardToRemove will be nul
            if (cardToRemove == null) return;

            cards.Remove(cardToRemove);                                                 // If a VCard is found, it is removed from the list of cards

            var newContent = string.Join(Environment.NewLine, cards.Select(c => c.ToString()));     // All remaining VCards in the card list are converted to
                                                                                                    // their string representation again (c.ToString())

            _fileStore.WriteAllText(_filePath, newContent + Environment.NewLine);       // The entire `_filePath` file is overwritten with the `newContent`. 
                                                                                        // This is not the most efficient method of deletion for very large files, 
                                                                                        // as it requires reading the entire file into memory, deleting one item, 
                                                                                        // and then overwriting the entire file. For small collections of VCards, this is acceptable
        }

        public void exportCard(Guid id)                                                // standart
        {
            var card = GetVCard(id);
            if (card == null) return;

            var fileName = $"export_{card.FullName.Replace(" ", "_")}.vcf";            // make file by name: export_Name_Surname.vcf
            _fileStore.WriteAllText(fileName, card.ToString());                        // becuase there is no file, it's create and write card in new emty file.       
        }

        public VCard GetVCard(Guid id)
        {
            return getAllCards().FirstOrDefault(c => c.Id == id);                   // for small applications or contact files, the current implementation is
                                                                                    //  quite simple, clear and sufficient
        }

        public IEnumerable<VCard> getAllCards()                                     // This method is the central method for reading all VCards from a file
        {
            if (!_fileStore.Exist(_filePath)) return new List<VCard>();             // Checks if the `_filePath` file exists. If not, returns an empty VCard list, 
                                                                                    // preventing read errors

            var allLines = _fileStore.ReadAllLines(_filePath);
            var cards = _cardProperty.parsedCards(allLines);

            return cards;       // At the end, the method returns all the collected VCards
        }

        public IEnumerable<VCard> FindByName(string name)
        {
            return getAllCards().Where(c => c.FullName.Contains(name, StringComparison.OrdinalIgnoreCase));
            ///<summary>
            ///     `getAllCards()` - It loads all VCards from a file and returns them as IEnumerable<VCard>
            /// 
            ///     `.Where(...)`   - This is a LINQ extension method. It filters the VCard collection returned by getAllCards()
            ///     based on the specified condition. Where creates a new collection containing only those items for which the condition is true.
            /// 
            ///     `c => c.FullName.Contains(name, StringComparison.OrdinalIgnoreCase)`
            ///                     - This is a lambda expression that defines a filtering condition for each VCard in the collection
            /// 
            ///     `StringComparison.OrdinalIgnoreCase`
            ///                     - This enum specifies how the string comparison should be performed. In this case
            ///                     `OrdinalIgnoreCase` means that the comparison will be done in a case-insensitive manner
            ///                     (e.g., “john” will find “John”, ‘JOHN’, “jOhN”, etc.) 
            ///                     and without regard to cultural rules (i.e., strictly on numeric character values).
            ///                     This is usually a good choice for searching, as it makes it more flexible for the user
            ///<summary>
        }
    }
}
