namespace RagEvaluator.Retrieval.Metrics.Models;

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
    /// Оценки по каждой K метрики
    /// </summary>
    public required IReadOnlyDictionary<int, double> Values { get; init; }
}

