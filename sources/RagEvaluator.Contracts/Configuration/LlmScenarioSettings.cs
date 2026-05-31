namespace RagEvaluator.Contracts.Configuration;

/// <summary>
/// Параметры вызова LLM для конкретного сценария.
/// </summary>
public sealed class LlmScenarioSettings
{
    /// <summary>
    /// Имя провайдера из <see cref="LlmSettings.Providers"/>.
    /// </summary>
    public string Provider { get; set; } = "";

    /// <summary>
    /// Идентификатор модели у провайдера.
    /// </summary>
    public string Model { get; set; } = "";

    /// <summary>
    /// Температура генерации.
    /// </summary>
    public float Temperature { get; set; }

    /// <summary>
    /// Максимальное число токенов в ответе модели.
    /// </summary>
    public int MaxOutputTokens { get; set; }

    /// <summary>
    /// Максимальное число параллельных вызовов LLM в этом сценарии.
    /// </summary>
    public int MaxParallelLlmCalls { get; set; } = 1;

    /// <summary>
    /// Путь к дополнительному system-промпту (надстройка к основному). Необязателен; пустая строка — не используется.
    /// </summary>
    public string AdditionalSystemPromptPath { get; set; } = "";

    /// <summary>
    /// Число повторных попыток вызова LLM после неудачи (0 — только одна попытка, без повторов).
    /// </summary>
    public int MaxRetryCount { get; set; }

    /// <summary>
    /// Пауза в секундах между повторными попытками.
    /// </summary>
    public int RetryDelaySeconds { get; set; } = 1;
}
