namespace RagEvaluator.Retrieval.Metrics.Abstractions.Models;

/// <summary>
/// Базовый контекст для вычисления метрик качества ранжирования.
/// </summary>
public class EvaluationContextBase
{
    /// <summary>
    /// Уникальный идентификатор оценки / запроса.
    /// </summary>
    public required string EvaluationId { get; init; }

    /// <summary>
    /// Идентификаторы релевантных документов для данного запроса.
    /// Key - DocumentId, Value - степень релевантности.
    /// </summary>
    public required IReadOnlyDictionary<string, int> RelevantDocumentIds { get; init; }

    /// <summary>
    /// Идентификаторы документов, ранжированные системой по score по убыванию.
    /// Key - DocumentId, Value - score.
    /// </summary>
    public required IReadOnlyDictionary<string, double> RankedDocumentIdsByScoreDesc { get; init; }
}

