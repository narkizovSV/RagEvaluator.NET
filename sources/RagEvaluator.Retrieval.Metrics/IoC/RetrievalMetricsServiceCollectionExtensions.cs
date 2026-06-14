using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RagEvaluator.Retrieval.Metrics.Abstractions;
using RagEvaluator.Retrieval.Metrics.Interfaces;
using RagEvaluator.Retrieval.Metrics.Models.Configurations;
using RagEvaluator.Retrieval.Metrics.Models.Contexts;
using RagEvaluator.Retrieval.Metrics.Services;
using RagEvaluator.Retrieval.Metrics.Services.Metrics;

namespace RagEvaluator.Retrieval.Metrics.IoC;

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

        services.AddSingleton<PrecisionAtKMetric>();
        services.AddSingleton<RecallAtKMetrics>();
        services.AddSingleton<ReciprocalRankAtKMetric>();
        services.AddSingleton<NdcgAtKMetric>();

        services.AddSingleton<AveragePrecisionMetric>(sp =>
            new AveragePrecisionMetric(sp.GetRequiredService<PrecisionAtKMetric>()));

        services.AddSingleton<IEnumerable<ITopKMetricBase<EvaluationContextWithK>>>(sp =>
        [
            sp.GetRequiredService<PrecisionAtKMetric>(),
            sp.GetRequiredService<RecallAtKMetrics>(),
            sp.GetRequiredService<AveragePrecisionMetric>(),
            sp.GetRequiredService<ReciprocalRankAtKMetric>(),
            sp.GetRequiredService<NdcgAtKMetric>()
        ]);

        services.AddSingleton<MeanPrecisionAtKMetric>(sp =>
            new MeanPrecisionAtKMetric(sp.GetRequiredService<PrecisionAtKMetric>()));

        services.AddSingleton<MeanRecallAtKMetric>(sp =>
            new MeanRecallAtKMetric(sp.GetRequiredService<RecallAtKMetrics>()));

        services.AddSingleton<MeanAveragePrecisionAtKMetric>(sp =>
            new MeanAveragePrecisionAtKMetric(sp.GetRequiredService<AveragePrecisionMetric>()));

        services.AddSingleton<MeanReciprocalRankAtKMetric>(sp =>
            new MeanReciprocalRankAtKMetric(sp.GetRequiredService<ReciprocalRankAtKMetric>()));

        services.AddSingleton<MeanNdcgAtKMetric>(sp =>
            new MeanNdcgAtKMetric(sp.GetRequiredService<NdcgAtKMetric>()));

        services.AddSingleton<IEnumerable<IAggregateTopKMetric<EvaluationContextWithK>>>(sp =>
        [
            sp.GetRequiredService<MeanPrecisionAtKMetric>(),
            sp.GetRequiredService<MeanRecallAtKMetric>(),
            sp.GetRequiredService<MeanAveragePrecisionAtKMetric>(),
            sp.GetRequiredService<MeanReciprocalRankAtKMetric>(),
            sp.GetRequiredService<MeanNdcgAtKMetric>()
        ]);

        services.AddSingleton<IRetrievalMetricsEvaluator, RetrievalMetricsEvaluator>();

        return services;
    }
}
