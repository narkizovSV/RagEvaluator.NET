using RagEvaluator.Retrieval.Metrics.Abstractions.Interfaces;
using RagEvaluator.Retrieval.Metrics.Abstractions.Models;
using RagEvaluator.Utilities;

namespace RagEvaluator.Retrieval.Metrics;

public class AveragePrecisionMetric : ITopKMetric
{
    public string Name => SupportMetrics.AveragePrecisionAtK;

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
                MetricName = SupportMetrics.AveragePrecisionAtK,
                Value = 0d,
                K = context.K
            };
        }

        var sumPrecision = 0d;
        var relevantSoFar = 0;
        var rank = 1;

        foreach (var pair in context.RankedDocumentIdsByScoreDesc.Take(context.K))
        {
            if (RelevanceGain.IsRelevant(pair.Key, context.RelevantDocumentIds))
            {
                relevantSoFar++;
                sumPrecision += (double)relevantSoFar / rank;
            }

            rank++;
        }

        return new EvaluationResult
        {
            MetricName = SupportMetrics.AveragePrecisionAtK,
            Value = sumPrecision / totalRelevant,
            K = context.K
        };
    }
}
