using System;
using System.Collections.Generic;
using System.Linq;

namespace VCardManager.Core
{
    public class CardService : ICardService
    {
        private readonly List<VCard> _cards = new();
        private readonly string _filePath = "data/contacts.vcf";

        public void addCard(VCard card)
        {
            // if (card == null) throw new ArgumentNullException(nameof(card));
            _cards.Add(card);
        }

        public void deleteCard(Guid id)
        {
            var card = _cards.FirstOrDefault(c => c.Id == id);
            if (card != null)
                _cards.Remove(card);
        }

        public void exportCard(Guid id)
        {
            var card = _cards.FirstOrDefault(c => c.Id == id);
            if (card != null)
                File.AppendAllText(_filePath, card.ToString() + Environment.NewLine);
        }

        public VCard GetVCard(Guid id)
        {
            return _cards.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<VCard> getAllCards()
        {
            return _cards;
        }

        public IEnumerable<VCard> FindByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Enumerable.Empty<VCard>();

            return _cards.Where(c => c.FullName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
