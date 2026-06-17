using RagEvaluator.Retrieval.Metrics.Abstractions.Interfaces;
using RagEvaluator.Retrieval.Metrics.Abstractions.Models;
using RagEvaluator.Utilities;

namespace RagEvaluator.Retrieval.Metrics;

public class ReciprocalRankAtKMetric : ITopKMetric
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
