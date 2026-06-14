using RagEvaluator.Retrieval.Metrics.Abstractions;
using RagEvaluator.Retrieval.Metrics.Models;
using RagEvaluator.Retrieval.Metrics.Models.Contexts;
using RagEvaluator.Retrieval.Metrics.Utils;

namespace RagEvaluator.Retrieval.Metrics.Services.Metrics;

public class PrecisionAtKMetric : ITopKMetricBase<EvaluationContextWithK>
{
    public string Name => SupportMetrics.PrecisionAtK;

    public EvaluationResult Evaluate(EvaluationContextWithK context)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(context.RankedDocumentIdsByScoreDesc);
        ArgumentNullException.ThrowIfNull(context.RelevantDocumentIds);

        if (context.K <= 0)
            throw new ArgumentException("Значение K должно быть больше 0.", nameof(context.K));

        var relevantInTopK = context.RankedDocumentIdsByScoreDesc
            .Take(context.K)
            .Count(pair => RelevanceGain.IsRelevant(pair.Key, context.RelevantDocumentIds));

        return new EvaluationResult
        {
            MetricName = SupportMetrics.PrecisionAtK,
            Value = (double)relevantInTopK / context.K,
            K = context.K
        };
    }
}
