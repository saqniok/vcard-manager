namespace VCardManager.Core
{
    public interface IFacade
    {
        void AddVCard();
        void DeleteCard();
        void ExportCard();
        void ShowCard();
        void ShowAllCards();
        void FindByName();
    }
}