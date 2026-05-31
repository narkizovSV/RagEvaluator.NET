using RagEvaluator.Contracts.Enums;

namespace RagEvaluator.Contracts.Configuration;

/// <summary>
/// Методы доступа к настройкам <see cref="LlmSettings"/> без побочных эффектов.
/// </summary>
public static class LlmSettingsExtensions
{
    /// <summary>
    /// Получает настройки сценария по enum; ключ в конфиге — <see cref="LlmScenarioType.ToString"/>.
    /// </summary>
    /// <param name="settings">Корневая секция <c>Llm</c> из конфигурации.</param>
    /// <param name="scenario">Сценарий использования LLM.</param>
    /// <returns>Параметры сценария.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="settings"/> равен <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">
    /// Сценарий не найден в секции <c>Llm:Scenarios</c>.
    /// </exception>
    public static LlmScenarioSettings GetScenario(this LlmSettings settings, LlmScenarioType scenario)
    {
        ArgumentNullException.ThrowIfNull(settings);
        return settings.Scenarios.TryGetValue(scenario.ToString(), out var scenarioSettings)
            ? scenarioSettings
            : throw new InvalidOperationException(
                $"Scenario '{scenario}' is not configured under '{LlmSettings.SectionName}:Scenarios'.");
    }

    /// <summary>
    /// Получает настройки провайдера для указанного сценария (через <see cref="LlmScenarioSettings.Provider"/>).
    /// </summary>
    /// <param name="settings">Корневая секция <c>Llm</c> из конфигурации.</param>
    /// <param name="scenario">Сценарий использования LLM.</param>
    /// <returns>Параметры подключения к провайдеру.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="settings"/> равен <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">
    /// Сценарий не настроен или провайдер из <see cref="LlmScenarioSettings.Provider"/> отсутствует в <c>Llm:Providers</c>.
    /// </exception>
    public static LlmProviderSettings GetProviderForScenario(this LlmSettings settings, LlmScenarioType scenario)
    {
        ArgumentNullException.ThrowIfNull(settings);
        var scenarioSettings = settings.GetScenario(scenario);
        return settings.Providers.TryGetValue(scenarioSettings.Provider, out var provider)
            ? provider
            : throw new InvalidOperationException(
                $"Provider '{scenarioSettings.Provider}' for scenario '{scenario}' is not configured under '{LlmSettings.SectionName}:Providers'.");
    }
}
