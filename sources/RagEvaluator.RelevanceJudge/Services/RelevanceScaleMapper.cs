using RagEvaluator.RelevanceJudge.Interfaces;

namespace RagEvaluator.RelevanceJudge.Services;

public sealed class RelevanceScaleMapper : IRelevanceScaleMapper
{
    private readonly int _binaryThresholdInclusive;

    public RelevanceScaleMapper(int binaryThresholdInclusive = 4)
    {
        _binaryThresholdInclusive = binaryThresholdInclusive;
    }

    public (int Binary, int Ternary) Map(int sourceScore)
    {
        if (sourceScore < 0 || sourceScore > 10)
            throw new ArgumentOutOfRangeException(nameof(sourceScore), "Score must be in range 0..10.");

        int binary = sourceScore >= _binaryThresholdInclusive ? 1 : 0;

        int ternary = sourceScore switch
        {
            <= 3 => 0,
            <= 6 => 1,
            _ => 2
        };

        return (binary, ternary);
    }
}

