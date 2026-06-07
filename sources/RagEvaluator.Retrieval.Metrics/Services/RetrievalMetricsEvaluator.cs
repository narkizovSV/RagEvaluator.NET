using Microsoft.Extensions.Options;
using RagEvaluator.Retrieval.Metrics.Base;
using RagEvaluator.Retrieval.Metrics.Interfaces;
using RagEvaluator.Retrieval.Metrics.Models;
using RagEvaluator.Retrieval.Metrics.Models.Configurations;

namespace RagEvaluator.Retrieval.Metrics.Services;

public class RetrievalMetricsEvaluator : IRetrievalMetricsEvaluator
{
    private readonly IReadOnlyDictionary<string, IMetric<EvaluationContext>> _metricsByName;
    private readonly MetricSettings _settings;

    public RetrievalMetricsEvaluator(
        IEnumerable<IMetric<EvaluationContext>> metrics,
        IOptions<MetricSettings> settings)
    {
        ArgumentNullException.ThrowIfNull(metrics);
        ArgumentNullException.ThrowIfNull(settings);

        _metricsByName = metrics.ToDictionary(metric => metric.Name);
        _settings = settings.Value;
    }

    public EvaluationSummary Evaluate(EvaluationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (_settings.MetricNames.Length == 0)
            throw new InvalidOperationException("Список метрик в конфигурации не должен быть пустым.");

        var metricResults = new List<EvaluationResult>(_settings.MetricNames.Length);

        foreach (var metricName in _settings.MetricNames)
        {
            if (!_metricsByName.TryGetValue(metricName, out var metric))
                throw new InvalidOperationException($"Метрика '{metricName}' не зарегистрирована.");

            metricResults.Add(metric.Evaluate(context));
        }

        return new EvaluationSummary
        {
            EvaluationId = context.EvaluationId,
            MetricResults = metricResults
        };
    }
}
