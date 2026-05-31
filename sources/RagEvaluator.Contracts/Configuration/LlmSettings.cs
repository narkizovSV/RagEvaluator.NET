using RagEvaluator.Contracts.Enums;

namespace RagEvaluator.Contracts.Configuration;

/// <summary>
/// Корневая секция конфигурации LLM: провайдеры и сценарии использования.
/// </summary>
public sealed class LlmSettings
{
    /// <summary>
    /// Имя секции в <c>appsettings.json</c> для привязки через <see cref="Microsoft.Extensions.Options.Options"/>.
    /// </summary>
    public const string SectionName = "Llm";

    /// <summary>
    /// Словарь провайдеров; ключ — имя провайдера (например, <c>OpenRouter</c>).
    /// </summary>
    public Dictionary<string, LlmProviderSettings> Providers { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Словарь сценариев; ключ совпадает с <see cref="LlmScenarioType"/> (например, <c>RelevanceJudging</c>).
    /// </summary>
    public Dictionary<string, LlmScenarioSettings> Scenarios { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}
