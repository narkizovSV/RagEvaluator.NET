using Microsoft.Extensions.Options;
using RagEvaluator.Retrieval.Metrics.Abstractions;
using RagEvaluator.Retrieval.Metrics.Interfaces;
using RagEvaluator.Retrieval.Metrics.Models;
using RagEvaluator.Retrieval.Metrics.Models.Configurations;
using RagEvaluator.Retrieval.Metrics.Models.Contexts;
using RagEvaluator.Retrieval.Metrics.Utils;

namespace RagEvaluator.Retrieval.Metrics.Services;

public class RetrievalMetricsEvaluator : IRetrievalMetricsEvaluator
{
    private readonly MetricSettings _settings;
    private readonly IReadOnlyList<ITopKMetricBase<EvaluationContextWithK>> _topKMetrics;
    private readonly IReadOnlyList<IAggregateTopKMetric<EvaluationContextWithK>> _aggregateTopKMetrics;

    public RetrievalMetricsEvaluator(
        IOptions<MetricSettings> settings,
        IEnumerable<ITopKMetricBase<EvaluationContextWithK>> topKMetrics,
        IEnumerable<IAggregateTopKMetric<EvaluationContextWithK>> aggregateTopKMetrics)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(topKMetrics);
        ArgumentNullException.ThrowIfNull(aggregateTopKMetrics);

        _settings = settings.Value;
        _topKMetrics = topKMetrics.ToList();
        _aggregateTopKMetrics = aggregateTopKMetrics.ToList();
    }

    public async Task<BatchEvaluationSummary> Evaluate()
    {
        var contexts = await EvaluationContextFactory.Create(
            _settings.QrelsFilePath,
            _settings.RunFilePath);

        if (contexts.Count == 0)
            throw new ArgumentException("Список контекстов не должен быть пустым.", nameof(contexts));

        var requestedMetrics = _settings.MetricNames.ToHashSet(StringComparer.Ordinal);

        ValidateMetricNames(requestedMetrics);

        var enabledTopKMetrics = _topKMetrics
            .Where(metric => requestedMetrics.Contains(metric.Name))
            .ToList();

        var enabledAggregateMetrics = _aggregateTopKMetrics
            .Where(metric => requestedMetrics.Contains(metric.Name))
            .ToList();

        var querySummaries = contexts
            .Select(context => CreateQuerySummary(context, enabledTopKMetrics))
            .ToList();

        return new BatchEvaluationSummary
        {
            QuerySummaries = querySummaries,
            AggregatedMetricResults = ComputeAggregatedResults(contexts, enabledAggregateMetrics)
        };
    }

    private EvaluationSummary CreateQuerySummary(
        EvaluationContextBase context,
        IReadOnlyList<ITopKMetricBase<EvaluationContextWithK>> enabledTopKMetrics)
    {
        var metricResults = new List<EvaluationResult>();

        foreach (var k in _settings.TopKValues)
        {
            var contextWithK = new EvaluationContextWithK
            {
                EvaluationId = context.EvaluationId,
                RelevantDocumentIds = context.RelevantDocumentIds,
                RankedDocumentIdsByScoreDesc = context.RankedDocumentIdsByScoreDesc,
                K = k
            };

            foreach (var metric in enabledTopKMetrics)
                metricResults.Add(metric.Evaluate(contextWithK));
        }

        return new EvaluationSummary
        {
            EvaluationId = context.EvaluationId,
            MetricResults = metricResults
        };
    }

    private IReadOnlyList<EvaluationResult> ComputeAggregatedResults(
        IReadOnlyList<EvaluationContextBase> contexts,
        IReadOnlyList<IAggregateTopKMetric<EvaluationContextWithK>> enabledAggregateMetrics)
    {
        var aggregatedResults = new List<EvaluationResult>();

        foreach (var k in _settings.TopKValues)
        {
            var contextsWithK = contexts
                .Select(context => new EvaluationContextWithK
                {
                    EvaluationId = context.EvaluationId,
                    RelevantDocumentIds = context.RelevantDocumentIds,
                    RankedDocumentIdsByScoreDesc = context.RankedDocumentIdsByScoreDesc,
                    K = k
                })
                .ToList();

            foreach (var aggregateMetric in enabledAggregateMetrics)
                aggregatedResults.Add(aggregateMetric.Evaluate(contextsWithK, k));
        }

        return aggregatedResults;
    }

    private static void ValidateMetricNames(HashSet<string> requestedMetrics)
    {
        var supportedMetrics = new HashSet<string>(StringComparer.Ordinal)
        {
            SupportMetrics.PrecisionAtK,
            SupportMetrics.RecallAtK,
            SupportMetrics.Mrr,
            SupportMetrics.AveragePrecisionAtK,
            SupportMetrics.MeanAveragePrecisionAtK,
            SupportMetrics.NdcgAtK
        };

        foreach (var metricName in requestedMetrics)
        {
            if (!supportedMetrics.Contains(metricName))
                throw new InvalidOperationException($"Метрика '{metricName}' не поддерживается.");
        }
    }
}
