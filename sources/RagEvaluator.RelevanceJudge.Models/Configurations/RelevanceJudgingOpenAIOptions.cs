namespace RagEvaluator.RelevanceJudge.Models.Configurations;

/// <summary>
/// Настройки OpenAI клиента.
/// </summary>
public class RelevanceJudgingOpenAIOptions
{
    /// <summary>
    /// Базовый URL API провайдера.
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// API-ключ для доступа к провайдеру.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор модели, используемой для оценки релевантности.
    /// </summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Температура генерации ответа модели.
    /// Чем выше значение, тем менее детерминированным будет результат.
    /// </summary>
    public float Temperature { get; set; }

    /// <summary>
    /// Максимальное количество токенов в ответе модели.
    /// </summary>
    public int MaxOutputTokens { get; set; }

    /// <summary>
    /// Максимальное количество параллельных вызовов LLM.
    /// </summary>
    public int MaxParallelLlmCalls { get; set; }

    /// <summary>
    /// Максимальное количество повторных попыток при ошибке запроса.
    /// </summary>
    public int MaxRetryCount { get; set; }

    /// <summary>
    /// Задержка в секундах между повторными попытками.
    /// </summary>
    public int RetryDelaySeconds { get; set; }

    /// <summary>
    /// Путь к дополнительному system prompt, если он используется.
    /// </summary>
    public string? AdditionalSystemPromptPath { get; set; }
}
