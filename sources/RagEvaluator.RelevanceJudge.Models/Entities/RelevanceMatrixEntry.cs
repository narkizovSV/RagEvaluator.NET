namespace RagEvaluator.RelevanceJudge.Models.Entities;

/// <summary>
/// Одна запись матрицы релевантности: пара «вопрос — partition» и производные шкалы.
/// </summary>
public class RelevanceMatrixEntry
{
    /// <summary>
    /// Идентификатор вопроса.
    /// </summary>
    public required string QuestionId { get; set; }

    /// <summary>
    /// Текст вопроса.
    /// </summary>
    public required string QuestionText { get; set; }

    /// <summary>
    /// Идентификатор partition.
    /// </summary>
    public required string PartitionId { get; set; }

    /// <summary>
    /// Текст partition.
    /// </summary>
    public required string PartitionText { get; set; }

    /// <summary>
    /// Исходный балл релевантности.
    /// </summary>
    public int RelevanceScore { get; set; }

    /// <summary>
    /// Бинарное представление релевантности.
    /// </summary>
    public int BinaryRelevanceScore { get; set; }

    /// <summary>
    /// Тернарное представление релевантности.
    /// </summary>
    public int TernaryRelevanceScore { get; set; }
}
