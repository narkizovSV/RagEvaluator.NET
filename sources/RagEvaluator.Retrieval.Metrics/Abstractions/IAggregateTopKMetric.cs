using RagEvaluator.Retrieval.Metrics.Models;
using RagEvaluator.Retrieval.Metrics.Models.Contexts;

namespace RagEvaluator.Retrieval.Metrics.Abstractions;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TContext"></typeparam>
public interface IAggregateTopKMetric<in TContext> where TContext : EvaluationContextBase
{
    /// <summary>
    /// 
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="contexts"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    EvaluationResult Evaluate(IReadOnlyList<TContext> contexts, int k);
}
