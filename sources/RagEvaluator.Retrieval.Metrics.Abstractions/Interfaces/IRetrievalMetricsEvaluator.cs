using RagEvaluator.Retrieval.Metrics.Abstractions.Models;

namespace RagEvaluator.Retrieval.Metrics.Abstractions.Interfaces;

/// <summary>
/// Загружает данные и вычисляет метрики ранжирования.
/// </summary>
public interface IRetrievalMetricsEvaluator
{
    /// <summary>
    /// Загружает qrels/run, вычисляет запрошенные метрики и возвращает сводку.
    /// </summary>
    Task<BatchEvaluationSummary> EvaluateAsync(CancellationToken cancellationToken = default);
}

