namespace RagEvaluator.Contracts.Interfaces;

/// <summary>
/// Определяет контракт для чтения и записи сериализуемых объектов в файловой системе.
/// </summary>
public interface IObjectStorage
{
    /// <summary>
    /// Асинхронно читает файл и десериализует его содержимое в объект указанного типа.
    /// </summary>
    /// <typeparam name="T">Тип десериализуемого объекта.</typeparam>
    /// <param name="path">Относительный или абсолютный путь к файлу.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Десериализованный объект или <see langword="null"/>, если JSON содержит <see langword="null"/>.</returns>
    /// <exception cref="FileNotFoundException">Файл не найден.</exception>
    /// <exception cref="InvalidDataException">Содержимое файла не является валидным JSON для типа <typeparamref name="T"/>.</exception>
    Task<T?> ReadAsync<T>(string path, CancellationToken cancellationToken = default);

    /// <summary>
    /// Асинхронно сериализует переданный объект и сохраняет его в файл по указанному пути.
    /// </summary>
    /// <typeparam name="T">Тип сериализуемого объекта.</typeparam>
    /// <param name="path">Относительный или абсолютный путь к файлу.</param>
    /// <param name="value">Объект для записи.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <exception cref="InvalidDataException">Объект не удалось сериализовать в JSON.</exception>
    Task WriteAsync<T>(string path, T value, CancellationToken cancellationToken = default);
}
