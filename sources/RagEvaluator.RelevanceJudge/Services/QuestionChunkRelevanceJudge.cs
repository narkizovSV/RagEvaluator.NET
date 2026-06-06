using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RagEvaluator.RelevanceJudge.Interfaces;
using RagEvaluator.RelevanceJudge.Models.Configurations;
using RagEvaluator.RelevanceJudge.Models.Entities;
using RagEvaluator.RelevanceJudge.Prompts;
using RagEvaluator.RelevanceJudge.Utils;
using RagEvaluator.Utilities;
using System.Text.Json;

namespace RagEvaluator.RelevanceJudge.Services;

public class QuestionChunkRelevanceJudge : IQuestionChunkRelevanceJudge
{
    private readonly ChatOptions _chatOptions;
    private readonly RelevanceJudgingOptions _judgingSettings;

    private readonly IChatClient _chatClient;
    private readonly ILogger<QuestionChunkRelevanceJudge> _logger;

    private readonly string? _systemPromptOverlay;

    public QuestionChunkRelevanceJudge(
        IChatClient chatClient,
        IOptions<RelevanceJudgingOptions> judgingSettings,
        ILogger<QuestionChunkRelevanceJudge> logger)
    {
        ArgumentNullException.ThrowIfNull(chatClient);
        ArgumentNullException.ThrowIfNull(judgingSettings);
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;
        _chatClient = chatClient;
        _judgingSettings = judgingSettings.Value;

        _chatOptions = RelevanceJudgingChatOptionsFactory.Create(_judgingSettings);

        if (!string.IsNullOrEmpty(_judgingSettings.OpenAI.AdditionalSystemPromptPath))
            _systemPromptOverlay = TextFileStorage.Read(_judgingSettings.OpenAI.AdditionalSystemPromptPath);
    }

    public async Task<RelevanceRating?> JudgeAsync(string questionText, string partitionText, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(questionText);
        ArgumentException.ThrowIfNullOrWhiteSpace(partitionText);

        var messages = BuildMessages(questionText, partitionText);
        var maxAttempts = 1 + Math.Max(0, _judgingSettings.OpenAI.MaxRetryCount);

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var response = await _chatClient.GetResponseAsync(messages, _chatOptions, cancellationToken);

                if (TryParseRating(response.Text, out var rating))
                    return rating;

                _logger.LogWarning(
                    "LLM returned invalid relevance payload on attempt {Attempt}/{MaxAttempts}.",
                    attempt,
                    maxAttempts);

                _logger.LogDebug(
                    "Unexpected relevance response: {Response}",
                    response.Text);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    "LLM relevance judging failed on attempt {Attempt}/{MaxAttempts}.",
                    attempt,
                    maxAttempts);
            }

            if (attempt < maxAttempts)
            {
                var delay = TimeSpan.FromSeconds(Math.Max(0, _judgingSettings.OpenAI.RetryDelaySeconds));
                if (delay > TimeSpan.Zero)
                    await Task.Delay(delay, cancellationToken);
            }
        }

        var message = $"Failed to get a valid relevance rating from LLM after {maxAttempts} attempts.";

        _logger.LogError(
            "{Message}",
            message);

        throw new InvalidOperationException(message);
    }

    private IReadOnlyList<ChatMessage> BuildMessages(string questionText, string chunkText)
    {
        return
        [
            new ChatMessage(ChatRole.System, QuestionChunkUserPrompt.BuildSystemPrompt(_systemPromptOverlay)),
            new ChatMessage(ChatRole.User, QuestionChunkUserPrompt.BuildUserMessage(questionText, chunkText))
        ];
    }

    private bool TryParseRating(string? responseText, out RelevanceRating? rating)
    {
        rating = null;

        if (string.IsNullOrWhiteSpace(responseText))
            return false;

        try
        {
            rating = JsonSerializer.Deserialize<RelevanceRating>(
                responseText.Trim(),
                RelevanceJudgingChatOptionsFactory.RelevanceJudgingJsonSchemaOptions);

            return rating is not null;
        }
        catch (JsonException ex)
        {
            _logger.LogDebug(
                ex,
                "Failed to deserialize LLM response as {Type}. Response: {Response}",
                nameof(RelevanceRating),
                responseText);

            return false;
        }
    }

}
