using RagEvaluator.RelevanceJudge.Models.Entities;

namespace RagEvaluator.RelevanceJudge.Utils;

public static class RanxQrelsConverter
{
    public static Dictionary<string, Dictionary<string, int>> ToBinaryQrels(IEnumerable<RelevanceMatrixEntry> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        var result = new Dictionary<string, Dictionary<string, int>>();

        foreach (var entry in entries)
        {
            if (entry.BinaryRelevanceScore != 1)
                continue;

            if (string.IsNullOrWhiteSpace(entry.QuestionId))
                continue;

            if (string.IsNullOrWhiteSpace(entry.PartitionId))
                continue;

            if (!result.TryGetValue(entry.QuestionId, out var docs))
            {
                docs = new Dictionary<string, int>();
                result[entry.QuestionId] = docs;
            }

            docs[entry.PartitionId] = entry.BinaryRelevanceScore;
        }

        return result;
    }

    public static Dictionary<string, Dictionary<string, int>> ToTernaryQrels(
        IEnumerable<RelevanceMatrixEntry> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        var result = new Dictionary<string, Dictionary<string, int>>();

        foreach (var entry in entries)
        {
            if (entry.TernaryRelevanceScore <= 0)
                continue;

            if (string.IsNullOrWhiteSpace(entry.QuestionId))
                continue;

            if (string.IsNullOrWhiteSpace(entry.PartitionId))
                continue;

            if (!result.TryGetValue(entry.QuestionId, out var docs))
            {
                docs = new Dictionary<string, int>();
                result[entry.QuestionId] = docs;
            }

            docs[entry.PartitionId] = entry.TernaryRelevanceScore;
        }

        return result;
    }
}
