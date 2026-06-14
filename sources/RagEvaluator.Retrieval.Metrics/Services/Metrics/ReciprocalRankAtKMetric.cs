using RagEvaluator.Retrieval.Metrics.Abstractions;
using RagEvaluator.Retrieval.Metrics.Models;
using RagEvaluator.Retrieval.Metrics.Models.Contexts;
using RagEvaluator.Retrieval.Metrics.Utils;

namespace RagEvaluator.Retrieval.Metrics.Services.Metrics;

public class ReciprocalRankAtKMetric : ITopKMetricBase<EvaluationContextWithK>
{
    public string Name => SupportMetrics.Mrr;

    public EvaluationResult Evaluate(EvaluationContextWithK context)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(context.RankedDocumentIdsByScoreDesc);
        ArgumentNullException.ThrowIfNull(context.RelevantDocumentIds);

        if (context.K <= 0)
            throw new ArgumentException("Значение K должно быть больше 0.", nameof(context.K));

        var rank = 1;

        foreach (var pair in context.RankedDocumentIdsByScoreDesc.Take(context.K))
        {
            if (RelevanceGain.IsRelevant(pair.Key, context.RelevantDocumentIds))
            {
                return new EvaluationResult
                {
                    MetricName = Name,
                    Value = 1d / rank,
                    K = context.K
                };
            }

            rank++;
        }

        return new EvaluationResult
        {
            MetricName = Name,
            Value = 0d,
            K = context.K
        };
    }
}
