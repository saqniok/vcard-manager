namespace VCardManager.Core
{
    public class CardProperty
    {
        public List<VCard> parsedCards(string[] allLines)
        {
            var cards = new List<VCard>();
            var chunk = new List<string>();

            foreach (var line in allLines)
                ProcessLine(line, cards, ref chunk);

            return cards;
        }

        private void ProcessLine(string line, List<VCard> cards, ref List<string> chunk)
        {
            if (line == "BEGIN:VCARD")
                chunk = new List<string>();

            chunk.Add(line);

            if (line == "END:VCARD")
            {
                var joined = string.Join('\n', chunk);                  // All strings collected in chunk are concatenated back into a single string, 
                cards.Add(VCard.FromVcf(joined));
            }

        }
    }
}