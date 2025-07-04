using VCardManager.Core;
namespace VCardManager.Tests;

public class CardServiceTest
{
    private readonly InMemoryFileStore _fileStore;
    private readonly CardService _cardService;
    private readonly CardProperty _cardProperty;
    private readonly string _testFilePath = "data/contacts.vcf";
    public CardServiceTest()
    {
        _fileStore = new InMemoryFileStore();
        _cardProperty = new CardProperty();
        _cardService = new CardService(_fileStore, _cardProperty);
    }

    [Fact]
    public void AddCard_ShouldAddVCardToFile()
    {
        var card = new VCard()
        {
            FullName = "Petrosyan",
            PhoneNumber = "003",
            Email = "da@esli.by"
        };

        _cardService.addCard(card);

        var fileContent = _fileStore.ReadAllText(_testFilePath);

        // Asserts
        Assert.True(_fileStore.Exist(_testFilePath));       // checking if file exist
        Assert.Contains("BEGIN:VCARD", fileContent);
        Assert.Contains("END:VCARD", fileContent);
        Assert.Contains(card.FullName, fileContent);
        Assert.Contains(card.PhoneNumber, fileContent);
        Assert.Contains(card.Email, fileContent);
        Assert.Contains($"{card.Id}", fileContent);

        // Using GetVCard method

        var recievedCard = _cardService.GetVCard(card.Id);
        Assert.Equal(card.Id, recievedCard.Id);
        Assert.Equal(card.FullName, recievedCard.FullName);
        Assert.Equal(card.PhoneNumber, recievedCard.PhoneNumber);
        Assert.Equal(card.Email, recievedCard.Email);
    }

    [Fact]
    public void DeleteCard_ShouldRemoveCardFromFile()
    {
        var card1 = new VCard { Id = Guid.NewGuid(), FullName = "Mimi", PhoneNumber = "0432112233", Email = "mimi@example.com" };
        var card2 = new VCard { Id = Guid.NewGuid(), FullName = "Bibi", PhoneNumber = "0432112233", Email = "bibi@example.com" };
        var card3 = new VCard { Id = Guid.NewGuid(), FullName = "Kuku", PhoneNumber = "0432112233", Email = "kuku@example.com" };

        _cardService.addCard(card1);
        _cardService.addCard(card2);
        _cardService.addCard(card3);

        var beforeDeleteCard = _cardService.getAllCards().ToList();
        Assert.Equal(3, beforeDeleteCard.Count);
        Assert.Contains(beforeDeleteCard, c => c.Id == card1.Id);
        Assert.Contains(beforeDeleteCard, c => c.Id == card2.Id);
        Assert.Contains(beforeDeleteCard, c => c.Id == card3.Id);

        // After Remove
        _cardService.deleteCard(card2.Id);
        var afterDeleteCard = _cardService.getAllCards().ToList();
        Assert.Equal(2, afterDeleteCard.Count);
        Assert.Contains(afterDeleteCard, c => c.Id == card1.Id);
        Assert.DoesNotContain(afterDeleteCard, c => c.Id == card2.Id);
        Assert.Contains(afterDeleteCard, c => c.Id == card3.Id);

    }

    [Fact]
    public void GetAllCards_ShouldReturnEmptyList_WhenFileDoesNotExist()
    {
        var allCards = _cardService.getAllCards().ToList();

        Assert.NotNull(allCards);
        Assert.Empty(allCards);
    }

    [Fact]
    public void GetAllCards_ShouldReturnAllCards_WhenFileExists()
    {
        var card1 = new VCard { Id = Guid.NewGuid(), FullName = "First", PhoneNumber = "0432112233", Email = "first@example.com" };
        var card2 = new VCard { Id = Guid.NewGuid(), FullName = "Second", PhoneNumber = "0432112233", Email = "second@example.com" };

        _cardService.addCard(card1);
        _cardService.addCard(card2);

        var allCards = _cardService.getAllCards().ToList();

        Assert.NotNull(allCards);
        Assert.Equal(2, allCards.Count);
        Assert.Contains(allCards, c => c.Id == card1.Id);
        Assert.Contains(allCards, c => c.Id == card2.Id);
    }

    [Fact]
    public void ExportCard_ShouldCreateNewFile_WithContanctInfo()
    {
        var card1 = new VCard { Id = Guid.NewGuid(), FullName = "James", PhoneNumber = "0432112233", Email = "James@example.com" };
        var card2 = new VCard { Id = Guid.NewGuid(), FullName = "Bond", PhoneNumber = "0432112233", Email = "bond@example.com" };

        _cardService.addCard(card1);
        _cardService.addCard(card2);

        _cardService.exportCard(card2.Id);
        var expectedFileName = $"export_{card2.FullName.Replace(" ", "_")}.vcf";

        Assert.True(_fileStore.Exist(expectedFileName));

        var exportedContent = _fileStore.ReadAllText(expectedFileName);
        Assert.Equal(card2.ToString(), exportedContent);

        // check if, after exporting, there is information in the source file prior to deletion
        var allCards = _cardService.getAllCards().ToList();
        Assert.Equal(2, allCards.Count);
    }

    [Fact]
    public void FindByName_ShoulBeReturnMatchingCards()
    {
        var card1 = new VCard { Id = Guid.NewGuid(), FullName = "Balck", PhoneNumber = "0432112233", Email = "black@example.com" };
        var card2 = new VCard { Id = Guid.NewGuid(), FullName = "Dick", PhoneNumber = "0432112233", Email = "dick@example.com" };
        var card3 = new VCard { Id = Guid.NewGuid(), FullName = "Dickenson", PhoneNumber = "0432112233", Email = "valey@example.com" };

        _cardService.addCard(card1);
        _cardService.addCard(card2);
        _cardService.addCard(card3);

        var foundCards = _cardService.FindByName("dick").ToList();

        Assert.NotNull(foundCards);
        Assert.Equal(2, foundCards.Count);
        Assert.Contains(foundCards, c => c.Id == card2.Id);
        Assert.Contains(foundCards, c => c.Id == card3.Id);
        Assert.DoesNotContain(foundCards, c => c.Id == card1.Id);
    }
}