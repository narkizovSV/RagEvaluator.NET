using RagEvaluator.Retrieval.Metrics.Models;

namespace RagEvaluator.Retrieval.Metrics.Interfaces;

/// <summary>
/// Загружает данные из файлов и вычисляет метрики ранжирования.
/// </summary>
public interface IRetrievalMetricsEvaluator
{
    /// <summary>
    /// Вычисляет метрики для набора запросов с агрегацией MAP@K и MRR.
    /// </summary>
    Task<BatchEvaluationSummary> Evaluate();
}
