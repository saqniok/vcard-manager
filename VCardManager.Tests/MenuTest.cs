using VCardManager.Core;
using Moq;

namespace VCardManager.Tests
{
    public class MenuTests
    {
        private readonly Mock<IFacade> _mockFacade;
        private readonly Mock<IConsole> _mockConsole;
        private readonly Menu _menu;

        // Конструктор тестового класса, вызывается перед каждым тестом
        public MenuTests()
        {
            _mockFacade = new Mock<IFacade>();
            _mockConsole = new Mock<IConsole>();
            _menu = new Menu(_mockFacade.Object, _mockConsole.Object);
        }

        [Fact]
        public void Constructor_InitializesDependencies()
        {
            // Arrange и Act уже выполнены в конструкторе MenuTests

            // Assert
            Assert.NotNull(_menu);
        }

        [Fact]
        public void ShowOptions_WritesAllMenuOptionsToConsole()
        {
            // Arrange
            var stringWriter = new StringWriter();
            _mockConsole.Setup(c => c.WriteLine(It.IsAny<string>()))
                        .Callback<string>(s => stringWriter.WriteLine(s));

            // Act
            _menu.ShowOptions();

            // Assert
            var consoleOutput = stringWriter.ToString();

            // Проверяем, что все ожидаемые строки меню присутствуют в выводе
            Assert.Contains("\nMenu:", consoleOutput);
            Assert.Contains("1. Add Card", consoleOutput);
            Assert.Contains("2. Delete Card", consoleOutput);
            Assert.Contains("3. Export Card", consoleOutput);
            Assert.Contains("4. Find and Show Card", consoleOutput);
            Assert.Contains("5. Show All Cards", consoleOutput);
            Assert.Contains("0. Exit", consoleOutput);
            Assert.Contains("Add number in console:", consoleOutput);

            // Проверяем, что WriteLine был вызван 8 раз (7 строк меню + 1 пустая строка в начале)
            _mockConsole.Verify(c => c.WriteLine(It.IsAny<string>()), Times.Exactly(8));
        }
    }
}