using RagEvaluator.Retrieval.Metrics.Abstractions;

namespace RagEvaluator.Retrieval.Metrics.Services.Metrics;

public class MeanReciprocalRankAtKMetric : MeanTopKMetricBase
{
    public MeanReciprocalRankAtKMetric(ReciprocalRankAtKMetric reciprocalRankAtKMetric)
        : base(reciprocalRankAtKMetric)
    {
    }

    public override string Name => SupportMetrics.Mrr;
}
