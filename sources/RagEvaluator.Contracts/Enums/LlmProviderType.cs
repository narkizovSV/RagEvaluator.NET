namespace RagEvaluator.Contracts.Enums;

/// <summary>
/// Тип LLM-провайдера.
/// </summary>
public enum LlmProviderType
{
    /// <summary>
    /// OpenAI-совместимый API (OpenAI, OpenRouter, Azure OpenAI и т.п.).
    /// </summary>
    OpenAiCompatible,

    /// <summary>
    /// Ollama (локальный или удалённый).
    /// </summary>
    Ollama,
}
