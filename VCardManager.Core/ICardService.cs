namespace VCardManager.Core
{
    public interface ICardService
    {
        void addCard(VCard card);
        void deleteCard(Guid id);
        void exportCard(Guid id);
        VCard GetVCard(Guid id);
        IEnumerable<VCard> getAllCards();
        IEnumerable<VCard> FindByName(string name);
    }
}