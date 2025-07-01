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

/*
    #   `IEnumerable<VCard>`    - type of the returned value. IEnumerable<T> represents a collection of
                                elements of type T (in this case VCard) that can be enumerated (e.g., in a foreach loop). 
                                It specifies that the method will return a list or set of all VCard objects
            
    #   `ICardService`          - is a critical component in an architecture based on OOP and clean code principles. 
                                It defines the contract for the core business logic of your VCardManager, 
                                abstracting away the implementation details. This allows your application to be more 
                                modular, testable, and flexible to changes in the future
*/