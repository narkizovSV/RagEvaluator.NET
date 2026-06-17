namespace RagEvaluator.Retrieval.Metrics.Abstractions.Models;

/// <summary>
/// Параметры вычисления метрик без привязки к источнику данных.
/// </summary>
public sealed class RetrievalMetricsOptions
{
    /// <summary>
    /// Список значений <c>K</c>, для которых вычисляются метрики ранжирования.
    /// </summary>
    public required int[] TopKValues { get; init; }

    /// <summary>
    /// Список метрик, которые необходимо вычислить.
    /// </summary>
    public required string[] MetricNames { get; init; }

    /// <summary>
    /// Стратегии агрегации per-query значений: <c>Mean</c>, <c>Std</c>.
    /// </summary>
    public string[] AggregationTypes { get; init; } = [ "Mean" ];
}
