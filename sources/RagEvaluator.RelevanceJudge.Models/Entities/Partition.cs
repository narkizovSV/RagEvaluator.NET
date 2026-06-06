namespace RagEvaluator.RelevanceJudge.Models.Entities;

/// <summary>
/// Контентный чанк (partition), который участвует в retrieval и разметке релевантности.
/// </summary>
public class Partition
{
    /// <summary>
    /// Уникальный идентификатор чанка.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Текстовое содержимое чанка.
    /// </summary>
    public required string Text { get; init; }
}
