using RagEvaluator.Retrieval.Metrics.Abstractions;

namespace RagEvaluator.Retrieval.Metrics.Services.Metrics;

public class MeanAveragePrecisionAtKMetric : MeanTopKMetricBase
{
    public MeanAveragePrecisionAtKMetric(AveragePrecisionMetric averagePrecisionAtKMetric)
        : base(averagePrecisionAtKMetric)
    {
    }

    public override string Name => SupportMetrics.MeanAveragePrecisionAtK;
}
