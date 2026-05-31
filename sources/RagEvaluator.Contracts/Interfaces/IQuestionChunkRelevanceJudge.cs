using RagEvaluator.Core.Entities;

namespace RagEvaluator.Contracts.Interfaces;

/// <summary>
/// Оценивает релевантность одной пары «вопрос — partition» с помощью LLM.
/// </summary>
public interface IQuestionChunkRelevanceJudge
{
    /// <summary>
    /// Отправляет в LLM вопрос и текст partition и возвращает оценку релевантности.
    /// </summary>
    /// <param name="questionText">Текст вопроса.</param>
    /// <param name="partitionText">Текст partition.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Оценка релевантности или <see langword="null"/>, если LLM не вернула результат.</returns>
    Task<RelevanceRating?> JudgeAsync(string questionText, string partitionText, CancellationToken cancellationToken = default);
}
