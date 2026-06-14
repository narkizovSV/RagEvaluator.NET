using RagEvaluator.Retrieval.Metrics.Abstractions;
using RagEvaluator.Retrieval.Metrics.Models;
using RagEvaluator.Retrieval.Metrics.Models.Contexts;
using RagEvaluator.Retrieval.Metrics.Utils;

namespace RagEvaluator.Retrieval.Metrics.Services.Metrics;

public class RecallAtKMetrics : ITopKMetricBase<EvaluationContextWithK>
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
