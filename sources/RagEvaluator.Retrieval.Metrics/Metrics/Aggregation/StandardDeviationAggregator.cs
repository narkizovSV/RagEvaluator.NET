using RagEvaluator.Retrieval.Metrics.Abstractions.Interfaces;

namespace RagEvaluator.Retrieval.Metrics.Aggregation;

/// <summary>
/// Выборочное стандартное отклонение per-query значений.
/// </summary>
public sealed class StandardDeviationAggregator : IMetricAggregator
{
    public string Name => "Std";

    public double Aggregate(IReadOnlyList<double> values)
    {
        ArgumentNullException.ThrowIfNull(values);

        if (values.Count == 0)
            throw new ArgumentException("Список значений не должен быть пустым.", nameof(values));

        if (values.Count == 1)
            return 0d;

        var mean = values.Average();
        var sumSquaredDiff = values.Sum(value => (value - mean) * (value - mean));

        return Math.Sqrt(sumSquaredDiff / (values.Count - 1));
    }
}
