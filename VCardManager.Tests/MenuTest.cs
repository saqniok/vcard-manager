using Moq;
using VCardManager.Core;
using VCardManager.Tests._tools;

namespace VCardManager.Tests;

public class MenuTests
{

    [Fact]
    public void Menu_Six_Exits()
    {
        var rolodex = new Mock<IFacade>();
        var console = new ConsoleSpy();
        console.AddInput("0");
        new Menu(rolodex.Object, console).Run();
    }

    private Mock<IFacade> PickOnceFromMenu(string choice)
    {
        var rolodex = new Mock<IFacade>();
        var console = new ConsoleSpy();
        console.AddInput(choice);
        console.AddInput("0");
        new Menu(rolodex.Object, console).Run();
        return rolodex;
    }

    [Fact]
    public void Menu_One()
    {
        PickOnceFromMenu("1").Verify(a => a.AddVCard(), Times.Once);
    }

    [Fact]
    public void Menu_Two()
    {
        PickOnceFromMenu("2").Verify(a => a.DeleteCard(), Times.Once);
    }

    [Fact]
    public void Menu_Three()
    {
        PickOnceFromMenu("3").Verify(a => a.ExportCard(), Times.Once);
    }

    [Fact]
    public void Menu_Four()
    {
        PickOnceFromMenu("4").Verify(a => a.FindByName(), Times.Once);
    }

    [Fact]
    public void Menu_Five()
    {
        PickOnceFromMenu("5").Verify(a => a.ShowAllCards(), Times.Once);
    }
    [Fact]
    public void OutOfMenu()
    {
        var rolodex = new Mock<IFacade>();
        var console = new ConsoleSpy();

        console.AddInput("6");
        console.AddInput("0");

        new Menu(rolodex.Object, console).Run();

        Assert.Contains("Wrong number, Try again and again and again....", console.Output);

        rolodex.VerifyNoOtherCalls();
    }
}