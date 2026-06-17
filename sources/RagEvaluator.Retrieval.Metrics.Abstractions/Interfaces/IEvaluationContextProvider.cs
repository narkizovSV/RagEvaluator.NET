using RagEvaluator.Retrieval.Metrics.Abstractions.Models;

namespace RagEvaluator.Retrieval.Metrics.Abstractions.Interfaces;

/// <summary>
/// Загружает контексты оценки из внешних источников.
/// </summary>
public interface IEvaluationContextProvider
{
    /// <summary>
    /// Создаёт контексты оценки из qrels и run.
    /// </summary>
    Task<IReadOnlyList<EvaluationContextBase>> CreateAsync(
        string qrelsFilePath,
        string runFilePath,
        CancellationToken cancellationToken = default);
}
