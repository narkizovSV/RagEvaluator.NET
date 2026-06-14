using RagEvaluator.Retrieval.Metrics.Abstractions;

namespace RagEvaluator.Retrieval.Metrics.Services.Metrics;

public class MeanNdcgAtKMetric : MeanTopKMetricBase
{
    public MeanNdcgAtKMetric(NdcgAtKMetric ndcgAtKMetric)
        : base(ndcgAtKMetric)
    {
    }

    public override string Name => SupportMetrics.NdcgAtK;
}
