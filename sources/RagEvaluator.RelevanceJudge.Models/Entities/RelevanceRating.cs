namespace RagEvaluator.RelevanceJudge.Models.Entities;

/// <summary>
/// Оценка релевантности чанка к вопросу, присвоенная LLM-судьёй (judge).
/// </summary>
public class RelevanceRating
{
    /// <summary>
    /// Минимально допустимый балл релевантности.
    /// </summary>
    public const int MinScore = 0;

    /// <summary>
    /// Максимально допустимый балл релевантности.
    /// </summary>
    public const int MaxScore = 10;

    /// <summary>
    /// Числовая оценка релевантности по шкале разметки (0–10).
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// Проверяет, что <see cref="Score"/> находится в допустимом диапазоне.
    /// </summary>
    public bool IsValid() => Score is >= MinScore and <= MaxScore;
}
