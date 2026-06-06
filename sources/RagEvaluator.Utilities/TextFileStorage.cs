using System.Text;

namespace RagEvaluator.Utilities;

/// <summary>
/// Предоставляет методы для чтения и записи текстовых файлов.
/// </summary>
public static class TextFileStorage
{
    /// <summary>
    /// Синхронно читает весь текст из файла.
    /// </summary>
    /// <param name="path">Путь к файлу.</param>
    /// <param name="baseDirectory">Базовая директория для относительных путей.</param>
    /// <param name="encoding">Кодировка файла. Если не указана, используется кодировка по умолчанию.</param>
    /// <returns>Содержимое файла в виде строки.</returns>
    /// <exception cref="ArgumentException">Выбрасывается, если путь пустой или состоит только из пробелов.</exception>
    /// <exception cref="FileNotFoundException">Выбрасывается, если файл не найден.</exception>
    public static string Read(
        string path,
        string? baseDirectory = null,
        Encoding? encoding = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var fullPath = ResolvePath(path, baseDirectory);
        if (!File.Exists(fullPath))
            throw new FileNotFoundException("Text file was not found.", fullPath);

        return encoding is null
            ? File.ReadAllText(fullPath)
            : File.ReadAllText(fullPath, encoding);
    }

    /// <summary>
    /// Асинхронно читает весь текст из файла.
    /// </summary>
    /// <param name="path">Путь к файлу.</param>
    /// <param name="baseDirectory">Базовая директория для относительных путей.</param>
    /// <param name="encoding">Кодировка файла. Если не указана, используется кодировка по умолчанию.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Содержимое файла в виде строки.</returns>
    /// <exception cref="ArgumentException">Выбрасывается, если путь пустой или состоит только из пробелов.</exception>
    /// <exception cref="FileNotFoundException">Выбрасывается, если файл не найден.</exception>
    public static async Task<string> ReadAsync(
        string path,
        string? baseDirectory = null,
        Encoding? encoding = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var fullPath = ResolvePath(path, baseDirectory);
        if (!File.Exists(fullPath))
            throw new FileNotFoundException("Text file was not found.", fullPath);

        return encoding is null
            ? await File.ReadAllTextAsync(fullPath, cancellationToken)
            : await File.ReadAllTextAsync(fullPath, encoding, cancellationToken);
    }

    /// <summary>
    /// Синхронно записывает текст в файл.
    /// Если директория назначения не существует, она будет создана.
    /// </summary>
    /// <param name="path">Путь к файлу.</param>
    /// <param name="content">Текстовое содержимое файла.</param>
    /// <param name="baseDirectory">Базовая директория для относительных путей.</param>
    /// <param name="encoding">Кодировка файла. Если не указана, используется кодировка по умолчанию.</param>
    /// <exception cref="ArgumentException">Выбрасывается, если путь пустой или состоит только из пробелов.</exception>
    /// <exception cref="ArgumentNullException">Выбрасывается, если содержимое равно null.</exception>
    public static void Write(
        string path,
        string content,
        string? baseDirectory = null,
        Encoding? encoding = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(content);

        var fullPath = ResolvePath(path, baseDirectory);
        EnsureDirectoryExists(fullPath);

        if (encoding is null)
            File.WriteAllText(fullPath, content);
        else
            File.WriteAllText(fullPath, content, encoding);
    }

    /// <summary>
    /// Асинхронно записывает текст в файл.
    /// Если директория назначения не существует, она будет создана.
    /// </summary>
    /// <param name="path">Путь к файлу.</param>
    /// <param name="content">Текстовое содержимое файла.</param>
    /// <param name="baseDirectory">Базовая директория для относительных путей.</param>
    /// <param name="encoding">Кодировка файла. Если не указана, используется кодировка по умолчанию.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <exception cref="ArgumentException">Выбрасывается, если путь пустой или состоит только из пробелов.</exception>
    /// <exception cref="ArgumentNullException">Выбрасывается, если содержимое равно null.</exception>
    public static async Task WriteAsync(
        string path,
        string content,
        string? baseDirectory = null,
        Encoding? encoding = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(content);

        var fullPath = ResolvePath(path, baseDirectory);
        EnsureDirectoryExists(fullPath);

        if (encoding is null)
            await File.WriteAllTextAsync(fullPath, content, cancellationToken);
        else
            await File.WriteAllTextAsync(fullPath, content, encoding, cancellationToken);
    }

    /// <summary>
    /// Синхронно дописывает текст в конец файла.
    /// Если файл или директория не существуют, они будут созданы.
    /// </summary>
    /// <param name="path">Путь к файлу.</param>
    /// <param name="content">Текст для добавления в конец файла.</param>
    /// <param name="baseDirectory">Базовая директория для относительных путей.</param>
    /// <param name="encoding">Кодировка файла. Если не указана, используется кодировка по умолчанию.</param>
    /// <exception cref="ArgumentException">Выбрасывается, если путь пустой или состоит только из пробелов.</exception>
    /// <exception cref="ArgumentNullException">Выбрасывается, если содержимое равно null.</exception>
    public static void Append(
        string path,
        string content,
        string? baseDirectory = null,
        Encoding? encoding = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(content);

        var fullPath = ResolvePath(path, baseDirectory);
        EnsureDirectoryExists(fullPath);

        if (encoding is null)
            File.AppendAllText(fullPath, content);
        else
            File.AppendAllText(fullPath, content, encoding);
    }

    /// <summary>
    /// Асинхронно дописывает текст в конец файла.
    /// Если файл или директория не существуют, они будут созданы.
    /// </summary>
    /// <param name="path">Путь к файлу.</param>
    /// <param name="content">Текст для добавления в конец файла.</param>
    /// <param name="baseDirectory">Базовая директория для относительных путей.</param>
    /// <param name="encoding">Кодировка файла. Если не указана, используется кодировка по умолчанию.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <exception cref="ArgumentException">Выбрасывается, если путь пустой или состоит только из пробелов.</exception>
    /// <exception cref="ArgumentNullException">Выбрасывается, если содержимое равно null.</exception>
    public static async Task AppendAsync(
        string path,
        string content,
        string? baseDirectory = null,
        Encoding? encoding = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(content);

        var fullPath = ResolvePath(path, baseDirectory);
        EnsureDirectoryExists(fullPath);

        if (encoding is null)
            await File.AppendAllTextAsync(fullPath, content, cancellationToken);
        else
            await File.AppendAllTextAsync(fullPath, content, encoding, cancellationToken);
    }

    /// <summary>
    /// Преобразует относительный или абсолютный путь в полный путь к файлу.
    /// </summary>
    /// <param name="path">Исходный путь.</param>
    /// <param name="baseDirectory">Базовая директория для относительных путей.</param>
    /// <returns>Полный путь к файлу.</returns>
    private static string ResolvePath(string path, string? baseDirectory)
    {
        if (string.IsNullOrWhiteSpace(baseDirectory))
            return Path.GetFullPath(path);

        var normalizedBaseDirectory = Path.GetFullPath(baseDirectory);
        return Path.GetFullPath(Path.Combine(normalizedBaseDirectory, path));
    }

    /// <summary>
    /// Создаёт директорию файла, если она ещё не существует.
    /// </summary>
    /// <param name="fullPath">Полный путь к файлу.</param>
    private static void EnsureDirectoryExists(string fullPath)
    {
        var directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrWhiteSpace(directory))
            Directory.CreateDirectory(directory);
    }
}