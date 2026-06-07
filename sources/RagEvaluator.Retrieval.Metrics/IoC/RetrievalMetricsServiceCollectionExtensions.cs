using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RagEvaluator.Retrieval.Metrics.Base;
using RagEvaluator.Retrieval.Metrics.Binary;
using RagEvaluator.Retrieval.Metrics.Interfaces;
using RagEvaluator.Retrieval.Metrics.Models;
using RagEvaluator.Retrieval.Metrics.Models.Configurations;
using RagEvaluator.Retrieval.Metrics.Services;
using RagEvaluator.Retrieval.Metrics.Ternary;

namespace RagEvaluator.Retrieval.Metrics.IoC;

/// <summary>
/// Методы расширения для регистрации сервисов модуля метрик качества поиска.
/// </summary>
public static class RetrievalMetricsServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует реализации метрик ранжирования и привязывает настройки из конфигурации.
    /// </summary>
    /// <param name="services">Коллекция сервисов приложения.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <returns>Обновлённая коллекция сервисов.</returns>
    public static IServiceCollection AddRetrievalMetrics(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MetricSettings>(
            configuration.GetSection(MetricSettings.SectionName));

        services.AddSingleton<IMetric<EvaluationContext>, PrecisionAtKMetric>();
        services.AddSingleton<IMetric<EvaluationContext>, TernaryPrecisionAtKMetric>();
        services.AddSingleton<IRetrievalMetricsEvaluator, RetrievalMetricsEvaluator>();

        return services;
    }
}
