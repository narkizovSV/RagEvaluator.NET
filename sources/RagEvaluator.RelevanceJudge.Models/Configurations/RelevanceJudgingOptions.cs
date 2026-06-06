namespace RagEvaluator.RelevanceJudge.Models.Configurations;

/// <summary>
/// Настройки оценки релевантности, загружаемые из конфигурации приложения.
/// </summary>
public class RelevanceJudgingOptions
{
    /// <summary>
    /// Имя секции конфигурации, содержащей настройки оценки релевантности.
    /// </summary>
    public const string SectionName = "RelevanceJudging";

    /// <summary>
    /// Путь к файлу с тестовыми вопросами для оценки релевантности.
    /// </summary>
    public required string QuestionsFilePath {  get; set; }

    /// <summary>
    /// Путь к файлу с партициями (фрагментами/чанками), по которым оценивается релевантность.
    /// </summary>
    public required string PartitionsFilePath { get; set; }

    /// <summary>
    ///  Настройки клиента OpenAI, используемого для оценки релевантности.
    /// </summary>
    public required RelevanceJudgingOpenAIOptions OpenAI {  get; set; }
}
