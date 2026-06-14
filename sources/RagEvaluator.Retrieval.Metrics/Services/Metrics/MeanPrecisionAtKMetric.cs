using RagEvaluator.Retrieval.Metrics.Abstractions;

namespace RagEvaluator.Retrieval.Metrics.Services.Metrics;

public class MeanPrecisionAtKMetric : MeanTopKMetricBase
{
    public MeanPrecisionAtKMetric(PrecisionAtKMetric precisionAtKMetric)
        : base(precisionAtKMetric)
    {
    }

    public override string Name => SupportMetrics.PrecisionAtK;
}
