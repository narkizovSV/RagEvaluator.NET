using RagEvaluator.Retrieval.Metrics.Abstractions;

namespace RagEvaluator.Retrieval.Metrics.Services.Metrics;

public class MeanRecallAtKMetric : MeanTopKMetricBase
{
    public MeanRecallAtKMetric(RecallAtKMetrics recallAtKMetric)
        : base(recallAtKMetric)
    {
    }

    public override string Name => SupportMetrics.RecallAtK;
}
