using RagEvaluator.Retrieval.Metrics.Base;
using RagEvaluator.Retrieval.Metrics.Constants;
using RagEvaluator.Retrieval.Metrics.Models;

namespace RagEvaluator.Retrieval.Metrics.Ternary;

public class TernaryPrecisionAtKMetric : IMetric<EvaluationContext>
{
    /// <summary>
    /// Максимальная оценка релевантности по умолчанию для тернарной шкалы (0, 1, 2).
    /// </summary>
    public const int DefaultMaxRelevanceGrade = 2;

    public string Name => SupportMetrics.TernaryPrecisionAtK;

    private readonly double _maxRelevanceGrade;

    public TernaryPrecisionAtKMetric()
        : this(DefaultMaxRelevanceGrade)
    {
    }

    public TernaryPrecisionAtKMetric(double maxRelevanceGrade)
    {
        _maxRelevanceGrade = maxRelevanceGrade;
    }

    public EvaluationResult Evaluate(EvaluationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(context.RankedDocumentIdsByScoreDesc);
        ArgumentNullException.ThrowIfNull(context.RelevantDocumentIds);
        ArgumentNullException.ThrowIfNull(context.K);

        if (context.K.Length == 0)
            throw new ArgumentException("Список K не должен быть пустым.", nameof(context));

        if (context.K.Any(k => k <= 0))
            throw new ArgumentException("Все значения K должны быть больше 0.", nameof(context));

        var values = context.K
            .Distinct()
            .OrderBy(k => k)
            .ToDictionary(
                k => k,
                k => ComputeScore(context, k));

        return new EvaluationResult
        {
            MetricName = Name,
            Values = values
        };
    }

    private double ComputeScore(EvaluationContext context, int k)
    {
        var relevanceWeightSum = context.RankedDocumentIdsByScoreDesc
            .Take(k)
            .Sum(pair => context.RelevantDocumentIds.TryGetValue(pair.Key, out var grade) ? grade : 0);

        var denominator = k * _maxRelevanceGrade;

        return (double)relevanceWeightSum / denominator;
    }
}
