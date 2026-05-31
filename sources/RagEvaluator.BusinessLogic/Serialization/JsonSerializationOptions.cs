using System.Text.Encodings.Web;
using System.Text.Json;

namespace RagEvaluator.BusinessLogic.Serialization;

/// <summary>
/// Единая точка настройки <see cref="JsonSerializerOptions"/> для всего приложения.
/// </summary>
public static class JsonSerializationOptions
{
    /// <summary>
    /// Генерация JSON Schema для structured output LLM (snake_case).
    /// </summary>
    public static JsonSerializerOptions LlmSchema { get; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    };

    /// <summary>
    /// Десериализация JSON-ответов LLM (snake_case, без учёта регистра имён).
    /// </summary>
    public static JsonSerializerOptions LlmResponse { get; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true,
    };

    /// <summary>
    /// Чтение и запись JSON-файлов (читаемый формат, relaxed escaping).
    /// </summary>
    public static JsonSerializerOptions FileStorage { get; } = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNameCaseInsensitive = true,
    };
}
