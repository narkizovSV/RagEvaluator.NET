using RagEvaluator.BusinessLogic.Serialization;
using RagEvaluator.Contracts.Interfaces;
using System.Text.Json;

namespace RagEvaluator.BusinessLogic.Services;

public class JsonFileObjectStorage : IObjectStorage
{
    private readonly string? _baseDirectory;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public JsonFileObjectStorage() : this(baseDirectory: null, jsonSerializerOptions: null)
    {
    }

    public JsonFileObjectStorage(string? baseDirectory, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        _baseDirectory = string.IsNullOrWhiteSpace(baseDirectory) ? null : Path.GetFullPath(baseDirectory);
        _jsonSerializerOptions = jsonSerializerOptions ?? JsonSerializationOptions.FileStorage;
    }

    public async Task<T?> ReadAsync<T>(string path, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var fullPath = ResolvePath(path);
        if (!File.Exists(fullPath))
            throw new FileNotFoundException("JSON file was not found.", fullPath);

        try
        {
            await using var stream = File.OpenRead(fullPath);
            return await JsonSerializer.DeserializeAsync<T>(stream, _jsonSerializerOptions, cancellationToken);
        }
        catch (JsonException ex)
        {
            throw new InvalidDataException($"Failed to deserialize JSON from '{fullPath}'.", ex);
        }
    }

    public async Task WriteAsync<T>(string path, T value, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(value);

        var fullPath = ResolvePath(path);

        try
        {
            var directory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrWhiteSpace(directory))
                Directory.CreateDirectory(directory);

            await using var stream = File.Create(fullPath);
            await JsonSerializer.SerializeAsync(stream, value, _jsonSerializerOptions, cancellationToken);
        }
        catch (JsonException ex)
        {
            throw new InvalidDataException($"Failed to serialize JSON to '{fullPath}'.", ex);
        }
    }

    private string ResolvePath(string path) =>
        _baseDirectory is null ? Path.GetFullPath(path) : Path.GetFullPath(Path.Combine(_baseDirectory, path));
}
