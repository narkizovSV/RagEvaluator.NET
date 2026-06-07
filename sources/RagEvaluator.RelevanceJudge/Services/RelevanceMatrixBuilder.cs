using Microsoft.Extensions.Options;
using RagEvaluator.RelevanceJudge.Interfaces;
using RagEvaluator.RelevanceJudge.Models.Configurations;
using RagEvaluator.RelevanceJudge.Models.Entities;
using System.Collections.Concurrent;

namespace RagEvaluator.RelevanceJudge.Services;

public class RelevanceMatrixBuilder : IRelevanceMatrixBuilder
{
    private readonly int _maxDegreeOfParallelism;

    private readonly IRelevanceScaleMapper _relevanceScaleMapper;
    private readonly IQuestionChunkRelevanceJudge _relevanceJudge;

    public RelevanceMatrixBuilder(
        IOptions<RelevanceJudgingOptions> judgingOptions,
        IRelevanceScaleMapper relevanceScaleMapper,
        IQuestionChunkRelevanceJudge relevanceJudge)
    {
        _maxDegreeOfParallelism = judgingOptions.Value.OpenAI.MaxParallelLlmCalls;

        _relevanceJudge = relevanceJudge;
        _relevanceScaleMapper = relevanceScaleMapper;
    }

    public async Task<List<RelevanceMatrixEntry>> BuildAsync(
        IReadOnlyList<Question> questions,
        IReadOnlyList<Partition> partitions,
        CancellationToken cancellationToken = default)
    {
        using var semaphore = new SemaphoreSlim(_maxDegreeOfParallelism, _maxDegreeOfParallelism);
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var linkedToken = linkedCts.Token;

        var rows = new ConcurrentBag<RelevanceMatrixEntry>();
        var tasks = new List<Task>(questions.Count * partitions.Count);

        for (var questionIndex = 0; questionIndex < questions.Count; questionIndex++)
        {
            for (var partitionIndex = 0; partitionIndex < partitions.Count; partitionIndex++)
            {
                tasks.Add(FillCellAsync(
                    questions[questionIndex],
                    partitions[partitionIndex],
                    semaphore,
                    rows,
                    linkedCts,
                    linkedToken));
            }
        }

        try
        {
            await Task.WhenAll(tasks);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // Внешний вызов попросил отмену — пробрасываем как есть.
            throw;
        }
        catch (OperationCanceledException)
        {
            // Отмена из-за internal cancel (ошибка в одной из задач).

            var failedTask = tasks.FirstOrDefault(t => t.IsFaulted && t.Exception is not null);
            var rootException = failedTask?.Exception?.Flatten().InnerExceptions.FirstOrDefault();

            throw new InvalidOperationException(
                "Не удалось построить матрицу релевантности из-за сбоя проверки релевантности и оставшиеся операции были отменены.",
                rootException);
        }

        return rows.ToList();
    }

    private async Task FillCellAsync(
        Question question,
        Partition partition,
        SemaphoreSlim semaphore,
        ConcurrentBag<RelevanceMatrixEntry> rows,
        CancellationTokenSource linkedCts,
        CancellationToken cancellationToken)
    {
        await semaphore.WaitAsync(cancellationToken);

        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            var relevanceCell = await _relevanceJudge.JudgeAsync(
                question.Text,
                partition.Text,
                cancellationToken);

            var scales = _relevanceScaleMapper.Map(relevanceCell.Score);

            rows.Add(new RelevanceMatrixEntry
            {
                QuestionId = question.Id,
                QuestionText = question.Text,
                PartitionId = partition.Id,
                PartitionText = partition.Text,
                RelevanceScore = relevanceCell.Score,
                BinaryRelevanceScore = scales.Binary,
                TernaryRelevanceScore = scales.Ternary
            });
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch
        {
            linkedCts.Cancel();
            throw;
        }
        finally
        {
            semaphore.Release();
        }
    }

}
