using RagEvaluator.Retrieval.Metrics.Abstractions.Interfaces;
using RagEvaluator.Retrieval.Metrics.Abstractions.Models;
using RagEvaluator.Utilities;

namespace RagEvaluator.Retrieval.Metrics;

public class RecallAtKMetrics : ITopKMetric
{
    public string Name => SupportMetrics.RecallAtK;

    public EvaluationResult Evaluate(EvaluationContextWithK context)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(context.RankedDocumentIdsByScoreDesc);
        ArgumentNullException.ThrowIfNull(context.RelevantDocumentIds);

        if (context.K <= 0)
            throw new ArgumentException("Значение K должно быть больше 0.", nameof(context.K));

        var totalRelevant = RelevanceGain.CountRelevant(context.RelevantDocumentIds);

        if (totalRelevant == 0)
        {
            return new EvaluationResult
            {
                MetricName = SupportMetrics.RecallAtK,
                Value = 0d,
                K = context.K
            };
        }

        var relevantInTopK = context.RankedDocumentIdsByScoreDesc
            .Take(context.K)
            .Count(pair => RelevanceGain.IsRelevant(pair.Key, context.RelevantDocumentIds));

        return new EvaluationResult
        {
            MetricName = SupportMetrics.RecallAtK,
            Value = (double)relevantInTopK / totalRelevant,
            K = context.K
        };
    }
}
