using Microsoft.Extensions.Options;
using RagEvaluator.Retrieval.Metrics.Abstractions.Interfaces;
using RagEvaluator.Retrieval.Metrics.Abstractions.Models;

namespace RagEvaluator.Retrieval.Metrics.Services;

internal sealed class RetrievalMetricsEvaluator : IRetrievalMetricsEvaluator
{
    private readonly MetricSettings _settings;
    private readonly IEvaluationContextProvider _contextProvider;
    private readonly IRetrievalMetricsCalculator _calculator;

    public RetrievalMetricsEvaluator(
        IOptions<MetricSettings> settings,
        IEvaluationContextProvider contextProvider,
        IRetrievalMetricsCalculator calculator)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(calculator);

        _settings = settings.Value;
        _contextProvider = contextProvider;
        _calculator = calculator;
    }

    public async Task<BatchEvaluationSummary> EvaluateAsync(CancellationToken cancellationToken = default)
    {
        var contexts = await _contextProvider.CreateAsync(
            _settings.QrelsFilePath,
            _settings.RunFilePath,
            cancellationToken);

        return _calculator.Evaluate(contexts, new RetrievalMetricsOptions
        {
            TopKValues = _settings.TopKValues,
            MetricNames = _settings.MetricNames,
            AggregationTypes = _settings.AggregationTypes
        });
    }
}
