using Microsoft.Extensions.AI;
using RagEvaluator.RelevanceJudge.Models.Configurations;
using RagEvaluator.RelevanceJudge.Models.Entities;
using System.Text.Json;

namespace RagEvaluator.RelevanceJudge.Extensions;

internal static class RelevanceJudgingChatOptionsExtensions
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
            ModelId = settings.Model,
            Temperature = settings.Temperature,
            MaxOutputTokens = settings.MaxOutputTokens,

            ResponseFormat = ChatResponseFormat.ForJsonSchema(
                AIJsonUtilities.CreateJsonSchema(
                    typeof(RelevanceRating),
                    serializerOptions: RelevanceJudgingJsonSchemaOptions),
                schemaName: nameof(RelevanceRating)),
        };
    }
}
