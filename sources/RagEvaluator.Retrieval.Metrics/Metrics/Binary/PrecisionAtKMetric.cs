using RagEvaluator.Retrieval.Metrics.Base;
using RagEvaluator.Retrieval.Metrics.Constants;
using RagEvaluator.Retrieval.Metrics.Models;

namespace RagEvaluator.Retrieval.Metrics.Binary;

public class PrecisionAtKMetric : IMetric<EvaluationContext>
{
    public string Name => SupportMetrics.PrecisionAtK;

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
                k =>
                {
                    var relevantInTopK = context.RankedDocumentIdsByScoreDesc
                        .Take(k)
                        .Count(pair => context.RelevantDocumentIds.ContainsKey(pair.Key));

                    return (double)relevantInTopK / k;
                });

        return new EvaluationResult
        {
            MetricName = Name,
            Values = values
        };
    }
}
