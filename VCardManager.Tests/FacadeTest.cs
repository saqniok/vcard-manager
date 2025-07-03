using VCardManager.Core;
using Moq; // to isolate the Facade class from its dependencies

namespace VCardManager.Tests
{
    public class FacadeTest
    {
        // Mocked dependencies
        private readonly Mock<ICardService> _mockCardService;
        private readonly Mock<IConsole> _mockConsole;
        private readonly Mock<IUserInteraction> _mockUserInteraction;
        private readonly Facade _facade;

        // Constructor
        public FacadeTest()
        {
            _mockCardService = new Mock<ICardService>();
            _mockConsole = new Mock<IConsole>();
            _mockUserInteraction = new Mock<IUserInteraction>();

            _facade = new Facade(
                _mockCardService.Object,
                _mockConsole.Object,
                _mockUserInteraction.Object
            );
        }

        // TESTS
        [Fact]
        public void SelectCardByName_NoCardsFound()
        {
            var prompt = "Enter name:";
            var userName = "blabla";
            _mockUserInteraction.Setup(ui => ui.GetUserInput(prompt)).Returns(userName);
            _mockCardService.Setup(cs => cs.FindByName(userName)).Returns(new List<VCard>()); // return empty List


            var stringWriter = new StringWriter();                                  // `StringWriter` is a class from the `System.IO` namespace. 
            _mockConsole.Setup(c => c.WriteLine(It.IsAny<string>()))                // It provides a way to write characters to a string. 
                        .Callback<string>(s => stringWriter.WriteLine(s));          // In this case, instead of outputting text directly to the actual console, 
                                                                                    // “redirect” in varbiable. After we can read text from there

            var result = _facade.SelectCardByName(prompt);

            // Assert
            Assert.Null(result);
            _mockUserInteraction.Verify(ui => ui.GetUserInput(prompt), Times.Once);     // Times.Once: This is an argument from Moq 
            _mockCardService.Verify(cs => cs.FindByName(userName), Times.Once);         // that indicates how many times this method is expected to be called
            Assert.Contains("Card not found.", stringWriter.ToString());                // Check console message
        }
        [Fact]
        public void SelectCardByName_ReturnMatchedName()
        {
            var prompt = "Enter name:";
            var userName = "Pikachu";
            var expectedCard = new VCard { FullName = userName, PhoneNumber = "123123", Email = "asdas" };
            _mockUserInteraction.Setup(ui => ui.GetUserInput(prompt)).Returns(userName);
            _mockCardService.Setup(cs => cs.FindByName(userName)).Returns(new List<VCard> { expectedCard });

            var result = _facade.SelectCardByName(prompt);

            Assert.NotNull(result);
            Assert.Equal(expectedCard, result);
            _mockUserInteraction.Verify(ui => ui.GetUserInput(prompt), Times.Once);
            _mockCardService.Verify(cs => cs.FindByName(userName), Times.Once);
            _mockConsole.Verify(c => c.WriteLine(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void SelectCardByName_ReturnMultiplyMatchedNames()
        {
            var prompt = "Enter name:";
            var userName = "Dick";

            var card1 = new VCard { FullName = "Dick", PhoneNumber = "123123", Email = "asdas" };
            var card2 = new VCard { FullName = "Dickenson", PhoneNumber = "123123", Email = "asdas" };

            _mockUserInteraction.Setup(ui => ui.GetUserInput(prompt)).Returns(userName);
            _mockCardService.Setup(cs => cs.FindByName(userName)).Returns(new List<VCard> { card1, card2 });

            // Simulate input "1"
            _mockConsole.SetupSequence(c => c.ReadLine()).Returns("1");

            var stringWriter = new StringWriter();
            _mockConsole.Setup(c => c.WriteLine(It.IsAny<string>())).Callback<string>(s => stringWriter.WriteLine(s));
            _mockConsole.Setup(c => c.Write(It.IsAny<string>())).Callback<string>(s => stringWriter.Write(s));

            var result = _facade.SelectCardByName(prompt);

            Assert.NotNull(result);
            Assert.Equal(card1, result);
            Assert.Contains("Find more Cards:", stringWriter.ToString());
            Assert.Contains("1. Dick, TEL: 123123, EMAIL: asdas", stringWriter.ToString());
            Assert.Contains("Add Card number:", stringWriter.ToString());
            _mockConsole.Verify(c => c.ReadLine(), Times.Once);
        }

        [Fact]
        public void AddVCard_CallsGetContactInfoAndAddCard()
        {
            var card = new VCard { FullName = "Dick", PhoneNumber = "123123", Email = "asdas" };
            _mockUserInteraction.Setup(ui => ui.GetContactInfoFromUser()).Returns(card);

            var stringWriter = new StringWriter();
            _mockConsole.Setup(c => c.WriteLine(It.IsAny<string>())).Callback<string>(s => stringWriter.WriteLine(s));

            _facade.AddVCard();

            _mockUserInteraction.Verify(ui => ui.GetContactInfoFromUser(), Times.Once);
            _mockCardService.Verify(cs => cs.addCard(card), Times.Once);
            Assert.Contains($"Card is added. Id: {card.Id}", stringWriter.ToString());
        }

        [Fact]
        public void DeleteCard_ShouldRemoveCard()
        {
            var promt = "Enter name of card for deleting: ";
            var deletingCard = new VCard { FullName = "Mick", PhoneNumber = "123123", Email = "asdas" };

            _mockUserInteraction.Setup(ui => ui.GetUserInput(promt)).Returns(deletingCard.FullName);
            _mockCardService.Setup(cs => cs.FindByName(deletingCard.FullName)).Returns(new List<VCard> { deletingCard });

            _mockConsole.SetupSequence(c => c.ReadLine()).Returns("y");

            var stringWriter = new StringWriter();
            _mockConsole.Setup(c => c.WriteLine(It.IsAny<string>())).Callback<string>(s => stringWriter.WriteLine(s));
            _mockConsole.Setup(c => c.Write(It.IsAny<string>())).Callback<string>(s => stringWriter.Write(s));

            _facade.DeleteCard();

            Assert.Contains($"Delete this card '{deletingCard.FullName}' (Id: {deletingCard.Id})? (y/n)", stringWriter.ToString());
            _mockConsole.Verify(c => c.ReadLine(), Times.Once);
            _mockCardService.Verify(cs => cs.deleteCard(deletingCard.Id), Times.Once);
            Assert.Contains("Card is deleted.", stringWriter.ToString());
        }

        [Fact]
        public void DeleteCard_ShouldReturnCanceledMessage()
        {
            var promt = "Enter name of card for deleting: ";
            var deletingCard = new VCard { FullName = "Mick", PhoneNumber = "123123", Email = "asdas" };

            _mockUserInteraction.Setup(ui => ui.GetUserInput(promt)).Returns(deletingCard.FullName);
            _mockCardService.Setup(cs => cs.FindByName(deletingCard.FullName)).Returns(new List<VCard> { deletingCard });

            _mockConsole.SetupSequence(c => c.ReadLine()).Returns("n");

            var stringWriter = new StringWriter();
            _mockConsole.Setup(c => c.WriteLine(It.IsAny<string>())).Callback<string>(s => stringWriter.WriteLine(s));
            _mockConsole.Setup(c => c.Write(It.IsAny<string>())).Callback<string>(s => stringWriter.Write(s));

            _facade.DeleteCard();

            Assert.Contains($"Delete this card '{deletingCard.FullName}' (Id: {deletingCard.Id})? (y/n)", stringWriter.ToString());
            _mockConsole.Verify(c => c.ReadLine(), Times.Once);
            Assert.Contains("Canceled", stringWriter.ToString());
        }
    }
}