namespace VCardManager.Core
{
    public interface IFileStore
    {
        bool Exist(string path);
        // Проверяет, существует ли файл по указанному path. Возвращает true, если существует
        string ReadAllText(string path);
        // Считывает весь текст из файла по указанному path. Возвращает содержимое файла в виде строки
        void WriteAllText(string path, string content);
        // Записывает указанный content в файл по path. Если файл существует, его содержимое будет перезаписано
        void AppendAllText(string path, string content);
        // Добавляет указанный content в конец файла по path. Если файл не существует, он будет создан
    }
}