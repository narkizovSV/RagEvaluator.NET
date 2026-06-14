using RagEvaluator.Retrieval.Metrics.Models.Contexts;

namespace RagEvaluator.Retrieval.Metrics.Abstractions;

/// <summary>
/// Специализированный контракт для метрик, использующих параметр K.
/// </summary>
public interface ITopKMetricBase<in TContext> : IMetricBase<TContext>
    where TContext : EvaluationContextBase, ITopKEvaluationContext
{
}
