using Moq;
using VCardManager.Core;

namespace VCardManager.Tests
{
    public class UserInteractionTest
    {
        private readonly Mock<IConsole> _mockConsole;
        private readonly UserInteraction _userInteraction;

        public UserInteractionTest()
        {
            _mockConsole = new Mock<IConsole>();
            _userInteraction = new UserInteraction(_mockConsole.Object);
        }

        [Fact]
        public void Constructor_InnitializesConsoleDependency()
        {
            Assert.NotNull(_userInteraction); // Checks if creates
        }

        [Fact]
        public void GetUserInput_WritePromptAndReturnInput()
        {
            var promt = "Enter name:";
            var expectedInput = "User";

            var stringWriter = new StringWriter();
            _mockConsole.Setup(c => c.WriteLine(It.IsAny<string>()))
                .Callback<string>(s => stringWriter.WriteLine(s));

            _mockConsole.Setup(c => c.ReadLine()).Returns(expectedInput);

            var result = _userInteraction.GetUserInput(promt);

            Assert.Equal(expectedInput, result);
            _mockConsole.Verify(c => c.WriteLine(promt), Times.Once);
            _mockConsole.Verify(c => c.ReadLine(), Times.Once);
            Assert.Contains(promt, stringWriter.ToString());
        }

        [Fact]
        public void GetContactInfoFromUser_PromtForInfoAndReturnVCard()
        {
            var expectName = "PiPi";
            var expectPhone = "123123";
            var expectEmail = "jonee@email";

            _mockConsole.SetupSequence(c => c.ReadLine())
                .Returns(expectName)
                .Returns(expectPhone)
                .Returns(expectEmail);

            var stringWriter = new StringWriter();
            _mockConsole.Setup(c => c.WriteLine(It.IsAny<string>()))
                .Callback<string>(s => stringWriter.WriteLine(s));

            var result = _userInteraction.GetContactInfoFromUser();

            Assert.Equal(expectName, result.FullName);
            Assert.Equal(expectPhone, result.PhoneNumber);
            Assert.Equal(expectEmail, result.Email);

            _mockConsole.Verify(c => c.WriteLine("Add name: "), Times.Once);
            _mockConsole.Verify(c => c.WriteLine("Add Phone Number: "), Times.Once);
            _mockConsole.Verify(c => c.WriteLine("Add email: "), Times.Once);

            _mockConsole.Verify(c => c.ReadLine(), Times.Exactly(3));

            var consoleOutput = stringWriter.ToString();
            Assert.Contains("Add name: ", consoleOutput);
            Assert.Contains("Add Phone Number: ", consoleOutput);
            Assert.Contains("Add email: ", consoleOutput);
        }

        [Theory]
        [InlineData("y", true, "Card is deleted. ")]
        [InlineData("Y", true, "Card is deleted. ")]
        [InlineData("yes", false, "Canceled")]
        [InlineData("n", false, "Canceled")]
        [InlineData("N", false, "Canceled")]
        [InlineData("", false, "Canceled")]
        [InlineData("anything", false, "Canceled")]
        public void ConfirmDelete_HandlesUserConfirmationCorrectly(string userInput, bool expectedResult, string expectedOutputMessage)
        {
            var card = new VCard { FullName = "Test Card", PhoneNumber = "111", Email = "test@example.com" };
            var expectedPrompt = $"Delete this card '{card.FullName}' (Id: {card.Id})? (y/n)";

            _mockConsole.Setup(c => c.ReadLine()).Returns(userInput);

            var stringWriter = new StringWriter();
            _mockConsole.Setup(c => c.WriteLine(It.IsAny<string>()))
                        .Callback<string>(s => stringWriter.WriteLine(s));
            _mockConsole.Setup(c => c.Write(It.IsAny<string>()))
                        .Callback<string>(s => stringWriter.Write(s));


            var result = _userInteraction.ConfirmDelete(card);

            Assert.Equal(expectedResult, result);
            _mockConsole.Verify(c => c.WriteLine(expectedPrompt), Times.Once);
            _mockConsole.Verify(c => c.ReadLine(), Times.Once);
            Assert.Contains(expectedOutputMessage, stringWriter.ToString());
        }
    }
}

