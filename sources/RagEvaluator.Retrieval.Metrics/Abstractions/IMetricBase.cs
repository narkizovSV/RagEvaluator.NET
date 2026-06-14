using RagEvaluator.Retrieval.Metrics.Models;
using RagEvaluator.Retrieval.Metrics.Models.Contexts;

namespace RagEvaluator.Retrieval.Metrics.Abstractions;

/// <summary>
/// Базовый контракт метрики качества.
/// </summary>
/// <typeparam name="TContext">Тип контекста вычисления метрики.</typeparam>
public interface IMetricBase<in TContext> where TContext : EvaluationContextBase
{
    /// <summary>
    /// Имя метрики.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Вычислить значение метрики для указанного контекста.
    /// </summary>
    /// <param name="context">Контекст вычисления.</param>
    /// <returns>Результат вычисления метрики.</returns>
    EvaluationResult Evaluate(TContext context);
}
