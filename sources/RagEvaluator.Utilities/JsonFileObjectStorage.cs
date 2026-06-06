using System.Text.Encodings.Web;
using System.Text.Json;

/// <summary>
/// Предоставляет методы для чтения и записи объектов в JSON-файлы.
/// </summary>
public static class JsonFileObjectStorage
{
    /// <summary>
    /// Параметры сериализации JSON для файлового хранения. Используется читаемый формат и relaxed escaping.
    /// </summary>
    public static JsonSerializerOptions FileStorage { get; } = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNameCaseInsensitive = true,
    };

    /// <summary>
    /// Асинхронно читает объект из JSON-файла.
    /// </summary>
    /// <typeparam name="T">Тип десериализуемого объекта.</typeparam>
    /// <param name="path">Путь к JSON-файлу.</param>
    /// <param name="baseDirectory">Базовая директория для относительных путей.</param>
    /// <param name="jsonSerializerOptions">Опции сериализации JSON.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Десериализованный объект или <see langword="null"/>, если JSON содержит null.</returns>
    /// <exception cref="ArgumentException">Выбрасывается, если путь пустой или состоит только из пробелов.</exception>
    /// <exception cref="FileNotFoundException">Выбрасывается, если файл не найден.</exception>
    /// <exception cref="InvalidDataException">Выбрасывается, если произошла ошибка десериализации JSON.</exception>
    public static async Task<T?> ReadAsync<T>(
        string path,
        string? baseDirectory = null,
        JsonSerializerOptions? jsonSerializerOptions = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var fullPath = ResolvePath(path, baseDirectory);
        if (!File.Exists(fullPath))
            throw new FileNotFoundException("JSON file was not found.", fullPath);

        try
        {
            await using var stream = File.OpenRead(fullPath);
            return await JsonSerializer.DeserializeAsync<T>(
                stream,
                jsonSerializerOptions ?? FileStorage,
                cancellationToken);
        }
        catch (JsonException ex)
        {
            throw new InvalidDataException($"Failed to deserialize JSON from '{fullPath}'.", ex);
        }
    }

    /// <summary>
    /// Асинхронно записывает объект в JSON-файл.
    /// </summary>
    /// <typeparam name="T">Тип сериализуемого объекта.</typeparam>
    /// <param name="path">Путь к JSON-файлу.</param>
    /// <param name="value">Объект для сериализации.</param>
    /// <param name="baseDirectory">Базовая директория для относительных путей.</param>
    /// <param name="jsonSerializerOptions">Опции сериализации JSON.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <exception cref="ArgumentException">Выбрасывается, если путь пустой или состоит только из пробелов.</exception>
    /// <exception cref="ArgumentNullException">Выбрасывается, если значение равно null.</exception>
    /// <exception cref="InvalidDataException">Выбрасывается, если произошла ошибка сериализации JSON.</exception>
    public static async Task WriteAsync<T>(
        string path,
        T value,
        string? baseDirectory = null,
        JsonSerializerOptions? jsonSerializerOptions = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(value);

        var fullPath = ResolvePath(path, baseDirectory);

        try
        {
            var directory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrWhiteSpace(directory))
                Directory.CreateDirectory(directory);

            await using var stream = File.Create(fullPath);
            await JsonSerializer.SerializeAsync(
                stream,
                value,
                jsonSerializerOptions ?? FileStorage,
                cancellationToken);
        }
        catch (JsonException ex)
        {
            throw new InvalidDataException($"Failed to serialize JSON to '{fullPath}'.", ex);
        }
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
}