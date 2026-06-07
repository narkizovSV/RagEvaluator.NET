using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RagEvaluator.RelevanceJudge.Interfaces;
using RagEvaluator.RelevanceJudge.Models.Configurations;
using RagEvaluator.RelevanceJudge.Services;

namespace RagEvaluator.RelevanceJudge.IoC;

/// <summary>
/// Методы расширения для регистрации сервисов модуля оценки релевантности.
/// </summary>
public static class RelevanceJudgeServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует сервисы модуля оценки релевантности и привязывает его настройки из конфигурации.
    /// </summary>
    /// <param name="services">Коллекция сервисов приложения.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <returns>Обновлённая коллекция сервисов.</returns>
    public static IServiceCollection AddRelevanceJudge(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RelevanceJudgingOptions>(
            configuration.GetSection(RelevanceJudgingOptions.SectionName));

        services.AddSingleton<IQuestionChunkRelevanceJudge, QuestionChunkRelevanceJudge>();
        services.AddSingleton<IRelevanceScaleMapper, RelevanceScaleMapper>();
        services.AddSingleton<IRelevanceMatrixBuilder, RelevanceMatrixBuilder>();
        services.AddSingleton<IQrelsExporter, QrelsExporter>();

        return services;
    }
}
