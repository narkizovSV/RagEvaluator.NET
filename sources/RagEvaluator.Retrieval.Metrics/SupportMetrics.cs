namespace RagEvaluator.Retrieval.Metrics;

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
    /// Бинарная метрика recall@K: доля найденных релевантных документов среди всех релевантных.
    /// </summary>
    public const string RecallAtK = "Recall@K";

    /// <summary>
    /// Reciprocal Rank@K для одного запроса; MRR — среднее при агрегации по запросам.
    /// </summary>
    public const string Mrr = "MRR";

    /// <summary>
    /// Average Precision@K для одного запроса.
    /// </summary>
    public const string AveragePrecisionAtK = "AP@K";

    /// <summary>
    /// Среднее Average Precision@K при агрегации по запросам.
    /// </summary>
    public const string MeanAveragePrecisionAtK = "MAP@K";

    /// <summary>
    /// Normalized Discounted Cumulative Gain@K с учётом градаций релевантности.
    /// </summary>
    public const string NdcgAtK = "nDCG@K";
}
