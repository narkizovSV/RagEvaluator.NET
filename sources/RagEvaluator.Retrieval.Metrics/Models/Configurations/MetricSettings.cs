namespace RagEvaluator.Retrieval.Metrics.Models.Configurations;

/// <summary>
/// Настройки приложения для оценки качества результатов поиска.
/// </summary>
public class MetricSettings
{
    /// <summary>
    /// Имя секции конфигурации, содержащей параметры приложения.
    /// </summary>
    public const string SectionName = "AppSettings";

    /// <summary>
    /// Список значений <c>K</c>, для которых вычисляются метрики ранжирования.
    /// </summary>
    public required int[] TopKValues { get; set; }

    /// <summary>
    /// Список метрик, которые необходимо вычислить.
    /// </summary>
    public required string[] MetricNames { get; set; }

    /// <summary>
    /// Путь к файлу с эталонными данными релевантности (<c>qrels</c>).
    /// Файл содержит для каждого вопроса список релевантных чанков или документов, которые считаются правильным ответом при оценке качества поиска.
    /// </summary>
    public required string QrelsFilePath { get; set; }

    /// <summary>
    /// Путь к файлу с результатами поиска системы (<c>run</c>).
    /// Файл содержит для каждого вопроса список чанков или документов, которые система вернула в результате RAG-поиска.
    /// </summary>
    public required string RunFilePath { get; set; }

    /// <summary>
    /// Путь к выходному файлу, в который будут сохранены результаты вычисления метрик.
    /// </summary>
    public required string OutputFilePath { get; set; }
}
