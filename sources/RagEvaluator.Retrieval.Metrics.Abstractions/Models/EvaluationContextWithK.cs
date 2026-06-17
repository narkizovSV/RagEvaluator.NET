using RagEvaluator.Retrieval.Metrics.Abstractions.Interfaces;

namespace RagEvaluator.Retrieval.Metrics.Abstractions.Models;

/// <summary>
/// Контекст для метрик вида @K.
/// </summary>
public sealed class EvaluationContextWithK : EvaluationContextBase, ITopKEvaluationContext
{
    /// <summary>
    /// Глубина ранжированного списка, до которой вычисляется метрика.
    /// </summary>
    public required int K { get; init; }
}
