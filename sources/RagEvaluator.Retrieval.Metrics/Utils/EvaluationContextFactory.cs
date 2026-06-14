using RagEvaluator.Retrieval.Metrics.Models.Contexts;

namespace RagEvaluator.Retrieval.Metrics.Utils;

/// <summary>
/// Строит <see cref="EvaluationContextBase"/> из qrels и run.
/// </summary>
public static class EvaluationContextFactory
{
    /// <summary>
    /// Создаёт контексты оценки для всех запросов, присутствующих в qrels и/или run.
    /// </summary>
    /// <param name="pathToQrelsPath"></param>
    /// <param name="pathToRunPath"></param>
    /// <returns></returns>
    public static async Task<IReadOnlyList<EvaluationContextBase>> Create(string pathToQrelsPath, string pathToRunPath)
    {
        ArgumentNullException.ThrowIfNull(pathToQrelsPath);
        ArgumentNullException.ThrowIfNull(pathToRunPath);

        var qrels = await JsonFileObjectStorage.ReadAsync<Dictionary<string, Dictionary<string, int>>>(pathToQrelsPath);
        var runs = await JsonFileObjectStorage.ReadAsync<Dictionary<string, Dictionary<string, double>>>(pathToRunPath);

        if (qrels.Count != runs.Count)        
            throw new InvalidDataException($"Количество ключей в qrels ({qrels.Count}) не совпадает с количеством ключей в run ({runs.Count}).");        

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
