using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RagEvaluator.Retrieval.Metrics.Abstractions.Interfaces;
using RagEvaluator.Retrieval.Metrics.Abstractions.Models;
using RagEvaluator.Retrieval.Metrics.Aggregation;
using RagEvaluator.Retrieval.Metrics.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace RagEvaluator.Retrieval.Metrics;

/// <summary>
/// Методы расширения для регистрации сервисов модуля метрик ранжирования.
/// </summary>
public static class RetrievalMetricsServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует сервисы модуля метрик ранжирования и привязывает настройки из конфигурации.
    /// </summary>
    public static IServiceCollection AddRetrievalMetrics(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MetricSettings>(
            configuration.GetSection(MetricSettings.SectionName));

        RegisterMetrics(services);
        RegisterAggregators(services);
        RegisterMetricDefinitions(services);

        services.AddSingleton<IEvaluationContextProvider, FileEvaluationContextProvider>();
        services.AddSingleton<IRetrievalMetricsCalculator, RetrievalMetricsCalculator>();
        services.AddSingleton<IRetrievalMetricsEvaluator, RetrievalMetricsEvaluator>();

        return services;
    }

    private static void RegisterMetrics(IServiceCollection services)
    {
        services.AddSingleton<PrecisionAtKMetric>();
        services.AddSingleton<RecallAtKMetrics>();
        services.AddSingleton<ReciprocalRankAtKMetric>();
        services.AddSingleton<NdcgAtKMetric>();
        services.AddSingleton<AveragePrecisionMetric>();
    }

    private static void RegisterAggregators(IServiceCollection services)
    {
        services.AddSingleton<IMetricAggregator, AverageAggregator>();
        services.AddSingleton<IMetricAggregator, StandardDeviationAggregator>();
    }

    private static void RegisterMetricDefinitions(IServiceCollection services)
    {
        services.AddSingleton<IEnumerable<TopKMetricDefinition>>(sp =>
        [
            CreateDefinition(
                sp.GetRequiredService<PrecisionAtKMetric>(),
                perQuery: [SupportMetrics.PrecisionAtK],
                aggregate: new Dictionary<string, string>
                {
                    [SupportMetrics.PrecisionAtK] = SupportMetrics.PrecisionAtK
                }),
            CreateDefinition(
                sp.GetRequiredService<RecallAtKMetrics>(),
                perQuery: [SupportMetrics.RecallAtK],
                aggregate: new Dictionary<string, string>
                {
                    [SupportMetrics.RecallAtK] = SupportMetrics.RecallAtK
                }),
            CreateDefinition(
                sp.GetRequiredService<ReciprocalRankAtKMetric>(),
                perQuery: [SupportMetrics.Mrr],
                aggregate: new Dictionary<string, string>
                {
                    [SupportMetrics.Mrr] = SupportMetrics.Mrr
                }),
            CreateDefinition(
                sp.GetRequiredService<AveragePrecisionMetric>(),
                perQuery: [SupportMetrics.AveragePrecisionAtK],
                aggregate: new Dictionary<string, string>
                {
                    [SupportMetrics.MeanAveragePrecisionAtK] = SupportMetrics.MeanAveragePrecisionAtK
                }),
            CreateDefinition(
                sp.GetRequiredService<NdcgAtKMetric>(),
                perQuery: [SupportMetrics.NdcgAtK],
                aggregate: new Dictionary<string, string>
                {
                    [SupportMetrics.NdcgAtK] = SupportMetrics.NdcgAtK
                })
        ]);
    }

    private static TopKMetricDefinition CreateDefinition(
        ITopKMetric metric,
        IReadOnlyList<string> perQuery,
        IReadOnlyDictionary<string, string> aggregate) => new()
        {
            Metric = metric,
            PerQueryConfigNames = perQuery.ToHashSet(StringComparer.Ordinal),
            AggregateConfigs = new Dictionary<string, string>(aggregate, StringComparer.Ordinal)
        };
}
