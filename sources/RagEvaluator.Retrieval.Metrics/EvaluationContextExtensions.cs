using RagEvaluator.Retrieval.Metrics.Abstractions.Models;

namespace RagEvaluator.Retrieval.Metrics;

internal static class EvaluationContextExtensions
{
    public static EvaluationContextWithK WithK(this EvaluationContextBase context, int k) => new()
    {
        EvaluationId = context.EvaluationId,
        RelevantDocumentIds = context.RelevantDocumentIds,
        RankedDocumentIdsByScoreDesc = context.RankedDocumentIdsByScoreDesc,
        K = k
    };
}
