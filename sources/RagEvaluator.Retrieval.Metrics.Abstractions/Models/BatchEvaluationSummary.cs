namespace RagEvaluator.Retrieval.Metrics.Abstractions.Models;

/// <summary>
/// Сводный результат вычисления метрик ранжирования по нескольким запросам.
/// </summary>
public class BatchEvaluationSummary
{
    /// <summary>
    /// Результаты метрик для каждого запроса.
    /// </summary>
    public required IReadOnlyList<EvaluationSummary> QuerySummaries { get; init; }

    /// <summary>
    /// Агрегированные метрики по всем запросам согласно K.
    /// </summary>
    public required IReadOnlyList<EvaluationResult> AggregatedMetricResults { get; init; }
}

