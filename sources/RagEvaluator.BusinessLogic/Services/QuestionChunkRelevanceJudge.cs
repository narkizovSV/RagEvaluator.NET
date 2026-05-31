using Microsoft.Extensions.AI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RagEvaluator.BusinessLogic.Prompts.RelevanceJudging;
using RagEvaluator.BusinessLogic.Serialization;
using RagEvaluator.Contracts.Configuration;
using RagEvaluator.Contracts.Enums;
using RagEvaluator.Contracts.Interfaces;
using RagEvaluator.Core.Entities;
using System.Text.Json;

namespace RagEvaluator.BusinessLogic.Services;

public class QuestionChunkRelevanceJudge : IQuestionChunkRelevanceJudge
{
    private const int MaxLoggedResponseLength = 500;

    private readonly IChatClient _chatClient;
    private readonly ILogger<QuestionChunkRelevanceJudge> _logger;

    private readonly ChatOptions _chatOptions;
    private readonly LlmScenarioSettings _scenario;

    private readonly string _systemPromptOverlay;

    public QuestionChunkRelevanceJudge(
        IChatClient chatClient,
        IOptions<LlmSettings> llmOptions,
        IHostEnvironment hostEnvironment,
        ILogger<QuestionChunkRelevanceJudge> logger)
    {
        ArgumentNullException.ThrowIfNull(chatClient);
        ArgumentNullException.ThrowIfNull(llmOptions);
        ArgumentNullException.ThrowIfNull(hostEnvironment);
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;
        _chatClient = chatClient;

        _scenario = llmOptions.Value.GetScenario(LlmScenarioType.RelevanceJudging);

        _chatOptions = RelevanceJudgingChatOptions.Create(_scenario);
        _systemPromptOverlay = _scenario.LoadAdditionalSystemPrompt(hostEnvironment.ContentRootPath, LlmScenarioType.RelevanceJudging);
    }

    public async Task<RelevanceRating?> JudgeAsync(string questionText, string partitionText, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(questionText);
        ArgumentException.ThrowIfNullOrWhiteSpace(partitionText);

        var messages = BuildMessages(questionText, partitionText);
        var maxAttempts = 1 + Math.Max(0, _scenario.MaxRetryCount);

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var rating = await RequestRatingAsync(messages, cancellationToken);
                if (rating is not null)
                    return rating;

                _logger.LogWarning(
                    "LLM returned invalid relevance payload on attempt {Attempt}/{MaxAttempts}.",
                    attempt,
                    maxAttempts);
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
                var delay = TimeSpan.FromSeconds(Math.Max(0, _scenario.RetryDelaySeconds));
                if (delay > TimeSpan.Zero)
                    await Task.Delay(delay, cancellationToken);
            }
        }

        _logger.LogError(
            "LLM relevance judging exhausted all {MaxAttempts} attempts.",
            maxAttempts);

        return null;
    }

    private async Task<RelevanceRating?> RequestRatingAsync(IReadOnlyCollection<ChatMessage> messages, CancellationToken cancellationToken)
    {
        var response = await _chatClient.GetResponseAsync(messages, _chatOptions, cancellationToken);

        if (TryParseRating(response.Text, out var rating))
            return rating;

        _logger.LogDebug(
            "Unexpected relevance response: {Response}",
            ShortenForLog(response.Text, MaxLoggedResponseLength));

        return null;
    }

    private bool TryParseRating(string? responseText, out RelevanceRating? rating)
    {
        rating = null;

        if (string.IsNullOrWhiteSpace(responseText))
            return false;

        try
        {
            rating = JsonSerializer.Deserialize<RelevanceRating>(responseText.Trim(), JsonSerializationOptions.LlmResponse);

            return rating is not null && rating.IsValid();
        }
        catch (JsonException ex)
        {
            _logger.LogDebug(
                ex,
                "Failed to deserialize LLM response as {Type}. Response: {Response}",
                nameof(RelevanceRating),
                ShortenForLog(responseText, MaxLoggedResponseLength));

            return false;
        }
    }

    private IReadOnlyList<ChatMessage> BuildMessages(string questionText, string partitionText)
    {
        return
        [
            new ChatMessage(ChatRole.System, QuestionChunkUserPrompt.BuildSystemPrompt(_systemPromptOverlay)),
            new ChatMessage(ChatRole.User, QuestionChunkUserPrompt.BuildUserMessage(questionText, partitionText))
        ];
    }

    private static string ShortenForLog(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            return value ?? string.Empty;

        return value[..maxLength] + "...";
    }
}
