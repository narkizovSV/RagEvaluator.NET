using RagEvaluator.Core.Entities;

namespace RagEvaluator.Contracts.Interfaces;

/// <summary>
/// Строит матрицу релевантности: для каждой пары «вопрос — partition» вызывает <see cref="IQuestionChunkRelevanceJudge"/>.
/// </summary>
public interface IRelevanceMatrixBuilder
{
    /// <summary>
    /// Формирует записи матрицы релевантности для всех комбинаций вопросов и partitions.
    /// </summary>
    /// <param name="questions">Список вопросов.</param>
    /// <param name="partitions">Список partitions.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Записи матрицы релевантности (по одной на пару вопрос — partition).</returns>
    Task<List<RelevanceMatrixEntry>> BuildAsync(IReadOnlyList<Question> questions, IReadOnlyList<Partition> partitions, CancellationToken cancellationToken = default);
}
