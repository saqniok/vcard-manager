namespace VCardManager.Core
{
    public interface IFileStore
    {
        bool Exist(string path);                            // no method body and a semicolon `;` at the end
                                                            // This is because the interface only declares the method, but does not implement it
                                                            // The implementation should be provided by the class that implements IFileStore
        string ReadAllText(string path);
        string[] ReadAllLines(string path);
        void WriteAllText(string path, string content);     // two parameters: `path` for the file path and `content` for the content to be written 
                                                            // This method, overwrites the file if it exists
        void AppendAllText(string path, string content);    // adds `content` to the end of the file if it exists, or creates a new file if it does not exist
    }
}

/*
    `interface` is a contract that defines a set of methods, properties, or events that any class that implements the interface must implement. 
    Interfaces do not contain method implementations; they describe only their signatures (name, parameters, return type).
*/

/*
Для чего нужен интерфейс?

Интерфейсы — это краеугольный камень полиморфизма и Dependency Inversion Principle (DIP) в ООП 

Они позволяют:
    - Абстрагировать реализацию: Вы можете работать с IFileStore, не зная, как именно хранятся данные (на диске, в облаке, в памяти и т.д.). Это делает ваш код более гибким
    - Упростить тестирование: Легко создать "фейковую" (mock) или "заглушку" (stub) реализацию IFileStore для тестов, чтобы не зависеть от реальной файловой системы
    - Изменить реализацию без изменения кода: Если вы решите, что вместо работы с локальными файлами вам нужно хранить данные в базе данных или на удаленном сервере, 
    вы просто создадите новый класс, который имплементирует IFileStore, и измените одну настройку, не затрагивая код, который использует IFileStore
*/

