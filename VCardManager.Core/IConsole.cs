namespace VCardManager.Core
{
    public interface IConsole
    {
        void WriteLine(string message);
        void Write(string message);
        string ReadLine();
    }
}

/*
Why do I need the IConsole interface?

## EN ## 

    Calling System.Console directly creates a tight coupling in your code. 
    This means that your code is directly bound to the console

    1. Testability: When testing business logic that interacts with the user, you don't need to run a 
    real console. You can create a “mock” implementation of IConsole that simply writes output to memory or
    returns predefined values for input, which greatly simplifies automated testing

    2. Flexibility: If in the future your application stops being a console application and becomes, for
    example, a web or GUI application, you don't have to change the logic that interacts with IConsole. 
    You will simply write a new implementation of IConsole for the web or GUI interface, and 
    the underlying code will remain unchanged.

    3. Separation of Concerns: 
    Separating I/O interaction from the core business logic makes your code cleaner and easier to understand and maintain.

## RU ## 

    Прямой вызов System.Console создает жесткую зависимость (tight coupling) в вашем коде. 
    Это значит, что ваш код напрямую привязан к консоли

    1. Тестируемость (Testability): При тестировании бизнес-логики, которая взаимодействует с пользователем, 
    вам не нужно запускать реальную консоль. Вы можете создать "фиктивную" (mock) реализацию IConsole, 
    которая просто записывает вывод в память или возвращает заранее заданные значения для ввода, 
    что значительно упрощает автоматизированное тестирование

    2. Гибкость (Flexibility): Если в будущем ваше приложение перестанет быть консольным и станет, 
    например, веб-приложением или приложением с графическим интерфейсом (GUI), вам не придется менять логику,
    которая взаимодействует с IConsole. Вы просто напишете новую реализацию IConsole для веб-интерфейса или 
    GUI, а основной код останется неизменным

    3. Разделение ответственности (Separation of Concerns): Отделение взаимодействия с вводом/выводом от
    основной бизнес-логики делает ваш код более чистым и легким для понимания и поддержки.
*/