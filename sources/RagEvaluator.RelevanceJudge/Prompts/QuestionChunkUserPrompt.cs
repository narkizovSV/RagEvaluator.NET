namespace RagEvaluator.RelevanceJudge.Prompts;

/// <summary>
/// Шаблоны сообщений для сценария <see cref="LlmScenarioType.RelevanceJudging"/>:
/// system-промпт (базовый + надстройка из конфигурации) и user-сообщение «вопрос — partition».
/// </summary>
public static class QuestionChunkUserPrompt
{
    private const string BaseSystemPrompt = """
        You are an expert relevance evaluator for Retrieval-Augmented Generation (RAG) systems.
        Your task is to assess how useful a given document chunk is for answering a user question.
        You will be given:
        - a user question
        - a single document chunk
        Return a relevance score on a scale from 0 to 10, where:
        - 0 = the chunk does not help answer the question at all;
        - 1–3 = the chunk is only weakly helpful or has slight topical overlap;
        - 4–6 = the chunk is related to the topic and contains partially useful information;
        - 7–8 = the chunk is strongly helpful for answering the question;
        - 9–10 = the chunk contains direct and highly useful information for the answer.
        Rules:
        - Evaluate meaning and usefulness, not just keyword overlap.
        - Use only the content of the given chunk.
        - Do not rely on external knowledge.
        - Do not add explanations.
        - Use the full scale, not only extreme values.
        Output format:
        ```json
        {"score": 0}
        ```
        """;

    /// <summary>
    /// Собирает system-промпт: базовый шаблон и опциональная надстройка инструкций.
    /// </summary>
    /// <param name="additionalOverlay">
    /// Дополнительные инструкции из <see cref="LlmScenarioSettings.AdditionalSystemPromptPath"/>.
    /// <see langword="null"/> или пустая строка — только базовый промпт.
    /// </param>
    /// <returns>Текст system-сообщения для запроса к LLM.</returns>
    public static string BuildSystemPrompt(string? additionalOverlay = null)
    {
        if (string.IsNullOrWhiteSpace(additionalOverlay))
            return BaseSystemPrompt.TrimEnd();

        return $"""
            {BaseSystemPrompt.TrimEnd()}

            Additional instructions:
            {additionalOverlay.Trim()}
            """;
    }

    /// <summary>
    /// Собирает текст user-сообщения: вопрос и partition (надстройка — только в system, не здесь).
    /// </summary>
    /// <param name="questionText">Текст вопроса.</param>
    /// <param name="partitionText">Текст partition.</param>
    /// <returns>Текст user-сообщения для запроса к модели.</returns>
    public static string BuildUserMessage(string questionText, string partitionText)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(questionText);
        ArgumentException.ThrowIfNullOrWhiteSpace(partitionText);

        return $"""
            Question:
            {questionText}

            Chunk:
            {partitionText}
            """;
    }
}
