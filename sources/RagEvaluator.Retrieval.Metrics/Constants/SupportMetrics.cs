namespace RagEvaluator.Retrieval.Metrics.Constants;

/// <summary>
/// Имена реализаций метрик ранжирования для использования в конфигурации (<c>appsettings.json</c>).
/// </summary>
public static class SupportMetrics
{
    /// <summary>
    /// Бинарная метрика precision@K: доля релевантных документов среди top-K.
    /// </summary>
    public const string PrecisionAtK = "Precision@K";

    /// <summary>
    /// Тернарная метрика precision@K: взвешенная доля релевантности среди top-K.
    /// </summary>
    public const string TernaryPrecisionAtK = "TernaryPrecision@K";
}
