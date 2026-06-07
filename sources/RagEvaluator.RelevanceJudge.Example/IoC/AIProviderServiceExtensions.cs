using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI;
using RagEvaluator.RelevanceJudge.Models.Configurations;
using System.ClientModel;

namespace RagEvaluator.ConsoleApp.IoC;

/// <summary>
/// Регистрация LLM-клиента для оценки релевантности.
/// </summary>
public static class AIProviderServiceExtensions
{
    public static IServiceCollection ConfigureAIProvider(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<OpenAIClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<RelevanceJudgingOptions>>().Value;

            return new OpenAIClient(
                new ApiKeyCredential(options.OpenAI.ApiKey),
                new OpenAIClientOptions
                {
                    Endpoint = new Uri(options.OpenAI.BaseUrl)
                });
        });

        services.AddSingleton<IChatClient>(sp =>
        {
            var model = configuration.GetValue<string>("RelevanceJudging:OpenAI:Model");

            if (string.IsNullOrWhiteSpace(model))
                throw new InvalidOperationException("RelevanceJudging:OpenAI:Model не задан в конфигурации.");

            return sp.GetRequiredService<OpenAIClient>()
                .GetChatClient(model)
                .AsIChatClient();
        });

        return services;
    }
}

