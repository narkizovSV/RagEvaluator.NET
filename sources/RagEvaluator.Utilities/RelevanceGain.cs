namespace RagEvaluator.Utilities;

/// <summary>
/// Вычисление gain для graded qrels (binary и ternary).
/// </summary>
public static class RelevanceGain
{
    /// <summary>
    /// Документ считается релевантным, если его grade > 0.
    /// </summary>
    public static bool IsRelevant(string documentId, IReadOnlyDictionary<string, int> qrels)
        => qrels.TryGetValue(documentId, out var relevance) && relevance > 0;

    /// <summary>
    /// Gain для nDCG: 2^rel − 1. Для rel ≤ 0 возвращает 0.
    /// </summary>
    public static double GetGain(string documentId, IReadOnlyDictionary<string, int> qrels)
    {
        if (!qrels.TryGetValue(documentId, out var relevance) || relevance <= 0)
            return 0d;

        return Math.Pow(2, relevance) - 1;
    }

    /// <summary>
    /// Число релевантных документов (grade > 0) в qrels.
    /// </summary>
    public static int CountRelevant(IReadOnlyDictionary<string, int> qrels)
        => qrels.Values.Count(relevance => relevance > 0);
}

