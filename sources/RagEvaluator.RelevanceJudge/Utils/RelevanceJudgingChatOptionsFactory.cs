using Microsoft.Extensions.AI;
using RagEvaluator.RelevanceJudge.Models.Configurations;
using RagEvaluator.RelevanceJudge.Models.Entities;
using System.Text.Json;

namespace RagEvaluator.RelevanceJudge.Utils;

internal static class RelevanceJudgingChatOptionsFactory
{
    public static JsonSerializerOptions RelevanceJudgingJsonSchemaOptions = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true
    };

    public static ChatOptions Create(RelevanceJudgingOptions settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return new ChatOptions
        {
            Temperature = settings.OpenAI.Temperature,
            MaxOutputTokens = settings.OpenAI.MaxOutputTokens,

            ResponseFormat = ChatResponseFormat.ForJsonSchema(
                AIJsonUtilities.CreateJsonSchema(
                    typeof(RelevanceRating),
                    serializerOptions: RelevanceJudgingJsonSchemaOptions),
                schemaName: nameof(RelevanceRating)),
        };
    }
}
