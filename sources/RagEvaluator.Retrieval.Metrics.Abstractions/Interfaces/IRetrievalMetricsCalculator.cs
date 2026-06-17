using RagEvaluator.Retrieval.Metrics.Abstractions.Models;

namespace RagEvaluator.Retrieval.Metrics.Abstractions.Interfaces;

/// <summary>
/// Вычисляет метрики ранжирования для готового набора контекстов.
/// </summary>
public interface IRetrievalMetricsCalculator
{
    /// <summary>
    /// Вычисляет per-query и агрегированные метрики.
    /// </summary>
    BatchEvaluationSummary Evaluate(
        IReadOnlyList<EvaluationContextBase> contexts,
        RetrievalMetricsOptions options);
}
