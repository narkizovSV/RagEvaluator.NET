namespace RagEvaluator.Retrieval.Metrics.Abstractions.Interfaces;

/// <summary>
/// Стратегия агрегации per-query значений метрики по нескольким запросам.
/// </summary>
public interface IMetricAggregator
{
    /// <summary>
    /// Имя стратегии (например, <c>Mean</c> или <c>Std</c>).
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Агрегировать список значений метрики.
    /// </summary>
    double Aggregate(IReadOnlyList<double> values);
}
