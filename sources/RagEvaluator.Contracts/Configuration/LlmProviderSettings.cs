using RagEvaluator.Contracts.Enums;

namespace RagEvaluator.Contracts.Configuration;

/// <summary>
/// Параметры подключения к LLM-провайдеру с OpenAI-совместимым API.
/// </summary>
public sealed class LlmProviderSettings
{
    /// <summary>
    /// Тип провайдера: какой клиент использовать для запросов.
    /// </summary>
    public LlmProviderType Type { get; set; }

    /// <summary>
    /// Базовый URL API (например, OpenRouter или OpenAI).
    /// </summary>
    public string BaseUrl { get; set; } = "";

    /// <summary>
    /// Ключ API для аутентификации запросов.
    /// </summary>
    public string ApiKey { get; set; } = "";
}
