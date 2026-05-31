using Microsoft.Extensions.AI;
using RagEvaluator.BusinessLogic.Serialization;
using RagEvaluator.Contracts.Configuration;
using RagEvaluator.Core.Entities;

namespace RagEvaluator.BusinessLogic.Prompts.RelevanceJudging;

/// <summary>
/// Параметры вызова LLM для сценария relevance judging.
/// </summary>
public static class RelevanceJudgingChatOptions
{
    public static ChatOptions Create(LlmScenarioSettings scenario)
    {
        ArgumentNullException.ThrowIfNull(scenario);

        return new ChatOptions
        {
            ModelId = scenario.Model,
            Temperature = scenario.Temperature,
            MaxOutputTokens = scenario.MaxOutputTokens,
            ResponseFormat = ChatResponseFormat.ForJsonSchema(
                AIJsonUtilities.CreateJsonSchema(
                    typeof(RelevanceRating),
                    serializerOptions: JsonSerializationOptions.LlmSchema),
                schemaName: nameof(RelevanceRating)),
        };
    }
}
