namespace RagEvaluator.Retrieval.Metrics.Abstractions.Models;

/// <summary>
/// Представляет результат вычисления одной метрики ранжирования для одного запроса или одной оценки.
/// </summary>
public class EvaluationResult
{
    /// <summary>
    /// Имя метрики
    /// </summary>
    public required string MetricName { get; init; }

    /// <summary>
    /// Оценка
    /// </summary>
    public required double Value { get; init; }

    /// <summary>
    /// Количество верхних документов ранжированного списка, используемых для расчёта.
    /// </summary>
    public int? K { get; init; }

    /// <summary>
    /// Тип агрегации (<c>Mean</c>, <c>Std</c>). Для per-query результатов — <c>null</c>.
    /// </summary>
    public string? AggregationType { get; init; }
}
