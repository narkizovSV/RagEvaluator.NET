using RagEvaluator.Retrieval.Metrics.Abstractions.Interfaces;

namespace RagEvaluator.Retrieval.Metrics.Abstractions.Models;

/// <summary>
/// Описание метрики: реализация и соответствие имён в конфигурации.
/// </summary>
public sealed class TopKMetricDefinition
{
    /// <summary>
    /// Реализация метрики.
    /// </summary>
    public required ITopKMetric Metric { get; init; }

    /// <summary>
    /// Имена в <c>MetricNames</c>, при которых метрика считается per-query.
    /// </summary>
    public required IReadOnlySet<string> PerQueryConfigNames { get; init; }

    /// <summary>
    /// Имена в <c>MetricNames</c> → имя метрики в агрегированных результатах.
    /// </summary>
    public required IReadOnlyDictionary<string, string> AggregateConfigs { get; init; }

    /// <summary>
    /// Все поддерживаемые имена для валидации конфигурации.
    /// </summary>
    public IEnumerable<string> AllConfigNames => PerQueryConfigNames.Concat(AggregateConfigs.Keys).Distinct();
}
