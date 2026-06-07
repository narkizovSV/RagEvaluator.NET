namespace RagEvaluator.Retrieval.Metrics.Models;

/// <summary>
/// 
/// </summary>
public class EvaluationContext
{
    /// <summary>
    /// Уникальный идентификатор оценки / запроса.
    /// </summary>
    public required string EvaluationId { get; init; }

    /// <summary>
    /// Идентификаторы релевантных документов для данного запроса.
    /// </summary>
    public required IReadOnlyDictionary<string, int> RelevantDocumentIds { get; init; }

    /// <summary>
    /// Идентификаторы документов, ранжированные системой по score по убыванию.
    /// </summary>
    public required IReadOnlyDictionary<string, double> RankedDocumentIdsByScoreDesc { get; init; }

    /// <summary>
    /// Список позиций, на которых нужно просчитать метрики
    /// </summary>
    public required int[] K { get; init; }
}
