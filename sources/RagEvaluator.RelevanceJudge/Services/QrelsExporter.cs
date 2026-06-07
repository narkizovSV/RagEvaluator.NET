using Microsoft.Extensions.Options;
using RagEvaluator.RelevanceJudge.Interfaces;
using RagEvaluator.RelevanceJudge.Models.Configurations;
using RagEvaluator.RelevanceJudge.Models.Entities;
using RagEvaluator.RelevanceJudge.Utils;

namespace RagEvaluator.RelevanceJudge.Services;

public class QrelsExporter : IQrelsExporter
{
    private readonly IOptions<RelevanceJudgingOptions> _judgingOptions;
    private readonly IRelevanceMatrixBuilder _matrixBuilder;

    public QrelsExporter(
        IOptions<RelevanceJudgingOptions> judgingOptions,
        IRelevanceMatrixBuilder matrixBuilder)
    {
        _judgingOptions = judgingOptions;
        _matrixBuilder = matrixBuilder;
    }

    public async Task ExportAsync(CancellationToken cancellationToken = default)
    {
        var options = _judgingOptions.Value;

        var questions = await JsonFileObjectStorage.ReadAsync<List<Question>>(
            options.QuestionsFilePath,
            cancellationToken: cancellationToken);

        var partitions = await JsonFileObjectStorage.ReadAsync<List<Partition>>(
            options.PartitionsFilePath,
            cancellationToken: cancellationToken);

        var result = await _matrixBuilder.BuildAsync(questions, partitions, cancellationToken);

        var binaryQrelsDataset = RanxQrelsConverter.ToBinaryQrels(result);
        var ternaryQrelsDataset = RanxQrelsConverter.ToTernaryQrels(result);

        await JsonFileObjectStorage.WriteAsync<Dictionary<string, Dictionary<string, int>>>(
            "qrels_binary.json",
            binaryQrelsDataset,
            baseDirectory: options.OutputDirectory,
            cancellationToken: cancellationToken);

        await JsonFileObjectStorage.WriteAsync<Dictionary<string, Dictionary<string, int>>>(
            "qrels_ternary.json",
            ternaryQrelsDataset,
            baseDirectory: options.OutputDirectory,
            cancellationToken: cancellationToken);
    }
}
