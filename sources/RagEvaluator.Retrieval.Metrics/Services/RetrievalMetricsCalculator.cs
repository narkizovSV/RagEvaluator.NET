using RagEvaluator.Retrieval.Metrics.Abstractions.Interfaces;
using RagEvaluator.Retrieval.Metrics.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RagEvaluator.Retrieval.Metrics.Services;

public sealed class RetrievalMetricsCalculator : IRetrievalMetricsCalculator
{
    private readonly IReadOnlyList<TopKMetricDefinition> _metricDefinitions;
    private readonly IReadOnlyList<IMetricAggregator> _aggregators;

    public RetrievalMetricsCalculator(
        IEnumerable<TopKMetricDefinition> metricDefinitions,
        IEnumerable<IMetricAggregator> aggregators)
    {
        ArgumentNullException.ThrowIfNull(metricDefinitions);
        ArgumentNullException.ThrowIfNull(aggregators);

        _metricDefinitions = metricDefinitions.ToList();
        _aggregators = aggregators.ToList();
    }

    public BatchEvaluationSummary Evaluate(
        IReadOnlyList<EvaluationContextBase> contexts,
        RetrievalMetricsOptions options)
    {
        ArgumentNullException.ThrowIfNull(contexts);
        ArgumentNullException.ThrowIfNull(options);

        if (contexts.Count == 0)
            throw new ArgumentException("Список контекстов не должен быть пустым.", nameof(contexts));

        var requestedMetrics = options.MetricNames.ToHashSet(StringComparer.Ordinal);

        ValidateMetricNames(requestedMetrics);

        var enabledAggregators = ResolveEnabledAggregators(options.AggregationTypes);

        var activeDefinitions = _metricDefinitions
            .Where(definition =>
                definition.PerQueryConfigNames.Any(requestedMetrics.Contains)
                || definition.AggregateConfigs.Keys.Any(requestedMetrics.Contains))
            .ToList();

        var querySummaries = contexts
            .Select(context => CreateQuerySummary(context, activeDefinitions, requestedMetrics, options.TopKValues))
            .ToList();

        return new BatchEvaluationSummary
        {
            QuerySummaries = querySummaries,
            AggregatedMetricResults = ComputeAggregatedResults(
                contexts,
                activeDefinitions,
                requestedMetrics,
                enabledAggregators,
                options.TopKValues)
        };
    }

    private static EvaluationSummary CreateQuerySummary(
        EvaluationContextBase context,
        IReadOnlyList<TopKMetricDefinition> activeDefinitions,
        IReadOnlySet<string> requestedMetrics,
        IReadOnlyList<int> topKValues)
    {
        var metricResults = new List<EvaluationResult>();

        foreach (var definition in activeDefinitions)
        {
            if (!definition.PerQueryConfigNames.Any(requestedMetrics.Contains))
                continue;

            foreach (var k in topKValues)
                metricResults.Add(definition.Metric.Evaluate(context.WithK(k)));
        }

        return new EvaluationSummary
        {
            EvaluationId = context.EvaluationId,
            MetricResults = metricResults
        };
    }

    private static IReadOnlyList<EvaluationResult> ComputeAggregatedResults(
        IReadOnlyList<EvaluationContextBase> contexts,
        IReadOnlyList<TopKMetricDefinition> activeDefinitions,
        IReadOnlySet<string> requestedMetrics,
        IReadOnlyList<IMetricAggregator> enabledAggregators,
        IReadOnlyList<int> topKValues)
    {
        var aggregatedResults = new List<EvaluationResult>();

        foreach (var definition in activeDefinitions)
        {
            var aggregateConfigs = definition.AggregateConfigs
                .Where(pair => requestedMetrics.Contains(pair.Key))
                .ToList();

            if (aggregateConfigs.Count == 0)
                continue;

            foreach (var k in topKValues)
            {
                var values = contexts
                    .Select(context => definition.Metric.Evaluate(context.WithK(k)).Value)
                    .ToList();

                foreach (var (_, resultName) in aggregateConfigs)
                {
                    foreach (var aggregator in enabledAggregators)
                    {
                        aggregatedResults.Add(new EvaluationResult
                        {
                            MetricName = resultName,
                            Value = aggregator.Aggregate(values),
                            K = k,
                            AggregationType = aggregator.Name
                        });
                    }
                }
            }
        }

        return aggregatedResults;
    }

    private IReadOnlyList<IMetricAggregator> ResolveEnabledAggregators(string[] requestedAggregationTypes)
    {
        var requestedTypes = requestedAggregationTypes.ToHashSet(StringComparer.Ordinal);
        var enabledAggregators = _aggregators
            .Where(aggregator => requestedTypes.Contains(aggregator.Name))
            .ToList();

        if (enabledAggregators.Count == 0)
            throw new InvalidOperationException("Не выбрана ни одна поддерживаемая стратегия агрегации.");

        foreach (var aggregationType in requestedTypes)
        {
            if (_aggregators.All(aggregator => aggregator.Name != aggregationType))
                throw new InvalidOperationException($"Стратегия агрегации '{aggregationType}' не поддерживается.");
        }

        return enabledAggregators;
    }

    private void ValidateMetricNames(HashSet<string> requestedMetrics)
    {
        var supportedMetrics = _metricDefinitions
            .SelectMany(definition => definition.AllConfigNames)
            .ToHashSet(StringComparer.Ordinal);

        foreach (var metricName in requestedMetrics)
        {
            if (!supportedMetrics.Contains(metricName))
                throw new InvalidOperationException($"Метрика '{metricName}' не поддерживается.");
        }
    }
}
