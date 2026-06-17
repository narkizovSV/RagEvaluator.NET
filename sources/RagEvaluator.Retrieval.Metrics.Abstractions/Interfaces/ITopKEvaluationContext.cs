namespace RagEvaluator.Retrieval.Metrics.Abstractions.Interfaces;

/// <summary>
/// Контракт для контекстов, в которых требуется параметр K.
/// </summary>
public interface ITopKEvaluationContext
{
    /// <summary>
    /// Глубина ранжированного списка, до которой вычисляется метрика.
    /// </summary>
    int K { get; }
}
