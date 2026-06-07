namespace RagEvaluator.RelevanceJudge.Interfaces;

/// <summary>
/// Строит матрицу релевантности и экспортирует qrels в JSON-файлы.
/// </summary>
public interface IQrelsExporter
{
    /// <summary>
    /// Читает вопросы и partitions, строит матрицу релевантности и сохраняет binary/ternary qrels.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    Task ExportAsync(CancellationToken cancellationToken = default);
}
