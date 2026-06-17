using RagEvaluator.Retrieval.Metrics.Abstractions.Interfaces;

namespace RagEvaluator.Retrieval.Metrics.Aggregation;

/// <summary>
/// Арифметическое среднее per-query значений.
/// </summary>
public sealed class AverageAggregator : IMetricAggregator
{
    public string Name => "Mean";

    public double Aggregate(IReadOnlyList<double> values)
    {
        ArgumentNullException.ThrowIfNull(values);

        if (values.Count == 0)
            throw new ArgumentException("Список значений не должен быть пустым.", nameof(values));

        return values.Average();
    }
}
