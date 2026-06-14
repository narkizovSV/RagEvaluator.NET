namespace RagEvaluator.Retrieval.Metrics.Models;

/// <summary>
/// Сводный результат вычисления всех метрик ранжирования для одного запроса.
/// </summary>
public class EvaluationSummary
{
    /// <summary>
    /// Уникальный идентификатор оценки / запроса.
    /// </summary>
    public required string EvaluationId { get; init; }

    /// <summary>
    /// Результаты вычисления каждой метрики.
    /// </summary>
    public required IReadOnlyList<EvaluationResult> MetricResults { get; init; }
}
