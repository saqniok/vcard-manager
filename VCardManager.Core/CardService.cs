using System;
using System.Collections.Generic;
using System.Linq;

namespace VCardManager.Core
{
    public class CardService : ICardService
    {
        //private readonly List<VCard> _cards = new();
        private readonly string _filePath = "data/contacts.vcf";

        public void addCard(VCard card)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));

            //_cards.Add(card);

            File.AppendAllText(_filePath, card.ToString() + Environment.NewLine);
        }

        public void deleteCard(Guid id)
        {
            // var card = _cards.FirstOrDefault(c => c.Id == id);
            // if (card != null)
            //     _cards.Remove(card);
            var cards = getAllCards().ToList();
            var cardToRemove = cards.FirstOrDefault(c => c.Id == id);
            if (cardToRemove == null) return;

            cards.Remove(cardToRemove);

            var newContent = string.Join(Environment.NewLine, cards.Select(c => c.ToString()));
            File.WriteAllText(_filePath, newContent + Environment.NewLine);
        }

        public void exportCard(Guid id)
        {
            // var card = _cards.FirstOrDefault(c => c.Id == id);
            // if (card != null)
            //     File.AppendAllText(_filePath, card.ToString() + Environment.NewLine);
            var card = GetVCard(id);
            if (card == null) return;

            var fileName = $"export_{card.FullName.Replace(" ", "_")}.vcf";
            File.WriteAllText(fileName, card.ToString());
        }

        public VCard GetVCard(Guid id)
        {
            //return _cards.FirstOrDefault(c => c.Id == id);
            return getAllCards().FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<VCard> getAllCards()
        {
            //return _cards;
            if (!File.Exists(_filePath)) return new List<VCard>();

            var allLines = File.ReadAllLines(_filePath);
            var cards = new List<VCard>();
            var chunk = new List<string>();

            foreach (var line in allLines)
            {
                if (line == "BEGIN:VCARD") chunk = new List<string>();
                chunk.Add(line);

                if (line == "END:VCARD")
                {
                    var joined = string.Join('\n', chunk);
                    cards.Add(VCard.FromVcf(joined));
                }
            }

            return cards;
        }

        public IEnumerable<VCard> FindByName(string name)
        {
            // if (string.IsNullOrWhiteSpace(name))
            //     return Enumerable.Empty<VCard>();

            // return _cards.Where(c => c.FullName.Contains(name, StringComparison.OrdinalIgnoreCase));
            return getAllCards().Where(c => c.FullName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
