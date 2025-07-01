// using System;
// using System.Collections.Generic;
// using System.Linq;

namespace VCardManager.Core
{
    public class CardService : ICardService                                             // Since we inherit the interface, we have to declare each method
    {
        private readonly string _filePath = "data/contacts.vcf";                        // `readonly` means that the value of this field can be set only when declaring it or
                                                                                        //  in the class constructor. It cannot be changed after that. 
                                                                                        //  This ensures that the path to the file remains constant

        public void addCard(VCard card)                                                 // This method adds a new VCard to the repository
        {
            if (card == null) throw new ArgumentNullException(nameof(card));            // `ArgumentNullException`, which prevents potential errors and indicates incorrect use of
                                                                                        //  the method. nameof(card) returns the name of the parameter as a string

            File.AppendAllText(_filePath, card.ToString() + Environment.NewLine);       // is used to add text to the end of the file specified in _filePath
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

            File.WriteAllText(_filePath, newContent + Environment.NewLine);             // The entire `_filePath` file is overwritten with the `newContent`. 
                                                                                        // This is not the most efficient method of deletion for very large files, 
                                                                                        // as it requires reading the entire file into memory, deleting one item, 
                                                                                        // and then overwriting the entire file. For small collections of VCards, this is acceptable
        }

        public void exportCard(Guid id)                                                // TODO:
        {
            var card = GetVCard(id);
            if (card == null) return;

            var fileName = $"export_{card.FullName.Replace(" ", "_")}.vcf";
            File.WriteAllText(fileName, card.ToString());
        }

        public VCard GetVCard(Guid id)
        {
            return getAllCards().FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<VCard> getAllCards()                                     // This method is the central method for reading all VCards from a file
        {
            if (!File.Exists(_filePath)) return new List<VCard>();                  // Checks if the `_filePath` file exists. If not, returns an empty VCard list, 
                                                                                    // preventing read errors

            var allLines = File.ReadAllLines(_filePath);                            // Reads all lines from the _filePath file into an array of strings
                                                                                    // return: A string array containing all lines of the file
            // Two lists are initialized: 
            var cards = new List<VCard>();              // `cards` for storing parsed VCard objects and `chunk` for temporarily storing 
            var chunk = new List<string>();             // strings belonging to one VCard block (between “BEGIN:VCARD” and “END:VCARD”)

            foreach (var line in allLines)                                  // Iterates over each line read from the file
            {
                if (line == "BEGIN:VCARD") chunk = new List<string>();      // If the current string is “BEGIN:VCARD”, this indicates the start of a new VCard. 
                chunk.Add(line);                                            // chunk is cleared to collect the lines of the new VCard

                if (line == "END:VCARD")                                    // If the current string is “END:VCARD”, it means the end of the current VCard block
                {
                    var joined = string.Join('\n', chunk);                  // All strings collected in chunk are concatenated back into a single string, 
                    cards.Add(VCard.FromVcf(joined));                       // separated by newline characters. This is needed to get a complete vcf block for parsing
                }
            }

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
