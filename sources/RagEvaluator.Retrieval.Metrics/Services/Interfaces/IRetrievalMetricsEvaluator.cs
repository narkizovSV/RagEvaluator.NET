using RagEvaluator.Retrieval.Metrics.Models;

namespace RagEvaluator.Retrieval.Metrics.Interfaces;

/// <summary>
/// Вычисляет все настроенные метрики ранжирования для одного контекста оценки.
/// </summary>
public interface IRetrievalMetricsEvaluator
{
    /// <summary>
    /// Рассчитывает метрики, указанные в конфигурации, и возвращает сводный результат.
    /// </summary>
    /// <param name="context">Контекст оценки: qrels, run и значения K.</param>
    /// <returns>Сводный объект с результатами всех метрик.</returns>
    EvaluationSummary Evaluate(EvaluationContext context);
}
