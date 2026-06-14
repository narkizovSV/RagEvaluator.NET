using RagEvaluator.Retrieval.Metrics.Abstractions;
using RagEvaluator.Retrieval.Metrics.Models;
using RagEvaluator.Retrieval.Metrics.Models.Contexts;
using RagEvaluator.Retrieval.Metrics.Utils;

namespace RagEvaluator.Retrieval.Metrics.Services.Metrics;

public class NdcgAtKMetric : ITopKMetricBase<EvaluationContextWithK>
{
    public string Name => SupportMetrics.NdcgAtK;

    public EvaluationResult Evaluate(EvaluationContextWithK context)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(context.RankedDocumentIdsByScoreDesc);
        ArgumentNullException.ThrowIfNull(context.RelevantDocumentIds);

        if (context.K <= 0)
            throw new ArgumentException("Значение K должно быть больше 0.", nameof(context.K));

        var dcg = ComputeDcg(context.RankedDocumentIdsByScoreDesc, context.RelevantDocumentIds, context.K);
        var idcg = ComputeIdealDcg(context.RelevantDocumentIds, context.K);

        return new EvaluationResult
        {
            MetricName = Name,
            Value = idcg > 0 ? dcg / idcg : 0d,
            K = context.K
        };
    }

    private static double ComputeDcg(
        IReadOnlyDictionary<string, double> rankedDocuments,
        IReadOnlyDictionary<string, int> qrels,
        int k)
    {
        var rank = 1;
        var dcg = 0d;

        foreach (var pair in rankedDocuments.Take(k))
        {
            dcg += RelevanceGain.GetGain(pair.Key, qrels) / Math.Log2(rank + 1);
            rank++;
        }

        return dcg;
    }

    private static double ComputeIdealDcg(IReadOnlyDictionary<string, int> qrels, int k)
    {
        var rank = 1;
        var idcg = 0d;

        foreach (var gain in qrels.Values
                     .Where(relevance => relevance > 0)
                     .Select(relevance => Math.Pow(2, relevance) - 1)
                     .OrderByDescending(value => value)
                     .Take(k))
        {
            idcg += gain / Math.Log2(rank + 1);
            rank++;
        }

        return idcg;
    }
}
