using RagEvaluator.Retrieval.Metrics.Abstractions;
using RagEvaluator.Retrieval.Metrics.Models;
using RagEvaluator.Retrieval.Metrics.Models.Contexts;
using RagEvaluator.Retrieval.Metrics.Utils;

namespace RagEvaluator.Retrieval.Metrics.Services.Metrics;

public class AveragePrecisionMetric : ITopKMetricBase<EvaluationContextWithK>
{
    private readonly ITopKMetricBase<EvaluationContextWithK> _precisionAtKMetric;

    public AveragePrecisionMetric(ITopKMetricBase<EvaluationContextWithK> precisionAtKMetric)
    {
        _precisionAtKMetric = precisionAtKMetric;
    }

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
        var rank = 1;

        foreach (var pair in context.RankedDocumentIdsByScoreDesc.Take(context.K))
        {
            if (RelevanceGain.IsRelevant(pair.Key, context.RelevantDocumentIds))
            {
                sumPrecision += _precisionAtKMetric.Evaluate(new EvaluationContextWithK
                {
                    EvaluationId = context.EvaluationId,
                    RelevantDocumentIds = context.RelevantDocumentIds,
                    RankedDocumentIdsByScoreDesc = context.RankedDocumentIdsByScoreDesc,
                    K = rank
                }).Value;
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
