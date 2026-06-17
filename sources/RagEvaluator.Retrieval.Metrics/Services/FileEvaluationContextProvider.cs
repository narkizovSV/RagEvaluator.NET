using RagEvaluator.Retrieval.Metrics.Abstractions.Interfaces;
using RagEvaluator.Retrieval.Metrics.Abstractions.Models;

namespace RagEvaluator.Retrieval.Metrics.Services;

/// <summary>
/// Загружает контексты оценки из JSON-файлов qrels и run.
/// </summary>
public sealed class FileEvaluationContextProvider : IEvaluationContextProvider
{
    public async Task<IReadOnlyList<EvaluationContextBase>> CreateAsync(
        string qrelsFilePath,
        string runFilePath,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(qrelsFilePath);
        ArgumentException.ThrowIfNullOrEmpty(runFilePath);

        var qrels = await JsonFileObjectStorage.ReadAsync<Dictionary<string, Dictionary<string, int>>>(
            qrelsFilePath,
            cancellationToken: cancellationToken);

        var runs = await JsonFileObjectStorage.ReadAsync<Dictionary<string, Dictionary<string, double>>>(
            runFilePath,
            cancellationToken: cancellationToken);

        if (qrels.Count != runs.Count)
        {
            throw new InvalidDataException(
                $"Количество ключей в qrels ({qrels.Count}) не совпадает с количеством ключей в run ({runs.Count}).");
        }

        if (!qrels.Keys.ToHashSet(StringComparer.Ordinal).SetEquals(runs.Keys))
            throw new InvalidDataException("Ключи запросов в qrels и run не совпадают.");

        var contexts = new List<EvaluationContextBase>(qrels.Count);

        foreach (var queryId in qrels.Keys)
        {
            var relevantDocuments = qrels[queryId];
            var rankedDocuments = runs[queryId];

            var rankedByScoreDesc = rankedDocuments
                .OrderByDescending(pair => pair.Value)
                .ThenBy(pair => pair.Key, StringComparer.Ordinal)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            contexts.Add(new EvaluationContextBase
            {
                EvaluationId = queryId,
                RelevantDocumentIds = relevantDocuments,
                RankedDocumentIdsByScoreDesc = rankedByScoreDesc
            });
        }

        return contexts;
    }
}

