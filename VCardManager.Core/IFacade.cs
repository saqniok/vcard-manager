namespace VCardManager.Core
{
    public interface IFacade
    {
        // These methods are declarations of the operations that Façade will provide to the client. Pay attention to several key points
        void AddVCard();            // No parameters: Most of these methods have no parameters. 
                                    // This is the hallmark of the Façade pattern when it interacts with the user
        void DeleteCard();          // void return type: All methods return void, which implies 
                                    // that they perform an action, but do not return a specific value directly to the calling code
        void ExportCard();          //
        void ShowCard();            //
        void ShowAllCards();        //
        void FindByName();          //
    }
}

/*
## RU ##
    Зачем нужен интерфейс IFacade и что такое паттерн "Фасад"?

    IFacade представляет собой реализацию паттерна проектирования "Фасад" (Facade Pattern). 
    Этот паттерн предназначен для предоставления упрощенного интерфейса к сложной подсистеме классов, библиотек или фреймворков.

    В данном контексте, IFacade скрывает сложность взаимодействия между различными компонентами VCardManager.Core 
    (такими как ICardService, IFileStore, IConsole и т.д.) от клиента (например, от вашего консольного приложения в VCardManager.CLI).

    # Преимущества паттерна "Фасад":

    ## Упрощение использования: 
        Вместо того чтобы клиенту приходилось напрямую взаимодействовать 
        с несколькими сложными объектами (сервисами, хранилищами, консолью), он просто вызывает 
        один метод на объекте Facade. Это значительно упрощает API для клиента.

    ## Снижение связанности (Loose Coupling): 
        Клиент теперь зависит только от Фасада, 
        а не от многих отдельных классов подсистемы. Это уменьшает связанность между 
        клиентом и сложной подсистемой.

    ## Инкапсуляция: 
        Фасад инкапсулирует логику взаимодействия с подсистемой. 
        Если внутренняя реализация подсистемы изменится, клиенту не нужно будет ничего менять, 
        если сигнатуры методов Фасада останутся прежними.

    ## Разделение ответственности: 
        Фасад может служить точкой входа для определенного набора операций, 
        что способствует лучшему разделению ответственности в приложении.


## EN ##
    Why do I need the IFacade interface and what is the Facade Pattern?

    IFacade is an implementation of the Facade Pattern. 
    This pattern is intended to provide a simplified interface to a complex subsystem of classes, libraries or frameworks

    In this context, IFacade hides the complexity of the interaction between the various components of VCardManager.Core
    (such as ICardService, IFileStore, IConsole, etc.) from the client (e.g., your console application in VCardManager.CLI)

    # Benefits of the Façade pattern:

    ## Usability simplification: 
        Instead of the client having to interact directly with several complex objects 
        (services, storage, console), it simply calls a single method on the Facade object. 
        This greatly simplifies the API for the client

    ## Loose Coupling: 
        The client now depends only on the Facade and not on many individual subsystem classes. 
        This reduces coupling between the client and the complex subsystem

    ## Encapsulation: 
        Facade encapsulates the logic of interaction with the subsystem. 
        If the internal implementation of the subsystem changes, the client does not need to change anything, 
        as long as the signatures of the Facade methods remain the same
    
    ## Separation of Responsibilities: 
        The façade can serve as an entry point for a specific set of operations, 
        which facilitates a better division of responsibilities in the application
*/