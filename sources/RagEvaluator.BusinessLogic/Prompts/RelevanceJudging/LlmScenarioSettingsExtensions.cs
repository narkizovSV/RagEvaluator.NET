using RagEvaluator.Contracts.Configuration;
using RagEvaluator.Contracts.Enums;

namespace RagEvaluator.BusinessLogic.Prompts.RelevanceJudging;

/// <summary>
/// Загрузка дополнительных инструкций system-промпта из файловой системы.
/// </summary>
public static class LlmScenarioSettingsExtensions
{
    /// <summary>
    /// Читает текст дополнительного system-промпта из <see cref="LlmScenarioSettings.AdditionalSystemPromptPath"/>.
    /// </summary>
    /// <param name="scenario">Настройки сценария.</param>
    /// <param name="contentRoot">Корневая директория приложения (например, <see cref="Microsoft.Extensions.Hosting.IHostEnvironment.ContentRootPath"/>).</param>
    /// <param name="scenarioType">Тип сценария (для сообщений об ошибках).</param>
    /// <returns>
    /// Содержимое файла надстройки или пустая строка, если <see cref="LlmScenarioSettings.AdditionalSystemPromptPath"/> не задан.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="scenario"/> равен <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="contentRoot"/> пустой.</exception>
    /// <exception cref="FileNotFoundException">Файл дополнительного system-промпта не найден.</exception>
    public static string LoadAdditionalSystemPrompt(this LlmScenarioSettings scenario, string contentRoot, LlmScenarioType scenarioType)
    {
        ArgumentNullException.ThrowIfNull(scenario);
        ArgumentException.ThrowIfNullOrWhiteSpace(contentRoot);

        var promptPath = scenario.AdditionalSystemPromptPath;
        if (string.IsNullOrWhiteSpace(promptPath))
            return string.Empty;

        var fullPath = Path.IsPathRooted(promptPath) ? promptPath : Path.GetFullPath(Path.Combine(contentRoot, promptPath));
        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Additional system prompt file not found for scenario '{scenarioType}': {fullPath}", fullPath);

        return File.ReadAllText(fullPath);
    }
}
