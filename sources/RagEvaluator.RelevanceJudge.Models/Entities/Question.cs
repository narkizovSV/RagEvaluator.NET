namespace RagEvaluator.RelevanceJudge.Models.Entities;

/// <summary>
/// Вопрос, для которого выполняется retrieval-оценка.
/// </summary>
public class Question
{
    /// <summary>
    /// Идентификатор вопроса.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Текст вопроса.
    /// </summary>
    public required string Text { get; init; }
}
