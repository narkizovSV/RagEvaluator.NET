namespace RagEvaluator.Contracts.Enums;

/// <summary>
/// Сценарий использования LLM в RagEvaluator.
/// </summary>
public enum LlmScenarioType
{
    /// <summary>
    /// Оценка релевантности пар «вопрос — чанк» (матрица релевантности).
    /// </summary>
    RelevanceJudging,

    /// <summary>
    /// Проверка faithfulness ответа относительно контекста.
    /// </summary>
    AnswerFaithfulness,

    /// <summary>
    /// Генерация ответа RAG для прогона оценки.
    /// </summary>
    Generation
}
