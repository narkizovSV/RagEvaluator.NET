using RagEvaluator.Retrieval.Metrics.Abstractions.Models;

namespace RagEvaluator.Retrieval.Metrics.Abstractions.Interfaces;

/// <summary>
/// Контракт метрики @K: вычисляет значение для одного запроса.
/// </summary>
public interface ITopKMetric
{
    /// <summary>
    /// Имя метрики в результатах per-query вычисления.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Вычислить значение метрики для указанного контекста.
    /// </summary>
    EvaluationResult Evaluate(EvaluationContextWithK context);
}

