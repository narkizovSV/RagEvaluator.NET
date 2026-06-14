using RagEvaluator.Retrieval.Metrics.Models;
using RagEvaluator.Retrieval.Metrics.Models.Contexts;

namespace RagEvaluator.Retrieval.Metrics.Abstractions;

/// <summary>
/// Базовый класс для агрегированных метрик @K: среднее значение per-query метрики по всем запросам.
/// </summary>
public abstract class MeanTopKMetricBase : IAggregateTopKMetric<EvaluationContextWithK>
{
    private readonly ITopKMetricBase<EvaluationContextWithK> _sourceMetric;

    protected MeanTopKMetricBase(ITopKMetricBase<EvaluationContextWithK> sourceMetric)
    {
        ArgumentNullException.ThrowIfNull(sourceMetric);
        _sourceMetric = sourceMetric;
    }

    public abstract string Name { get; }

    public EvaluationResult Evaluate(IReadOnlyList<EvaluationContextWithK> contexts, int k)
    {
        ArgumentNullException.ThrowIfNull(contexts);

        if (k <= 0)
            throw new ArgumentException("Значение K должно быть больше 0.", nameof(k));

        if (contexts.Count == 0)
            throw new ArgumentException("Список контекстов не должен быть пустым.", nameof(contexts));

        var meanValue = contexts
            .Select(context => _sourceMetric.Evaluate(new EvaluationContextWithK
            {
                EvaluationId = context.EvaluationId,
                RelevantDocumentIds = context.RelevantDocumentIds,
                RankedDocumentIdsByScoreDesc = context.RankedDocumentIdsByScoreDesc,
                K = k
            }).Value)
            .Average();

        return new EvaluationResult
        {
            MetricName = Name,
            Value = meanValue,
            K = k
        };
    }
}
