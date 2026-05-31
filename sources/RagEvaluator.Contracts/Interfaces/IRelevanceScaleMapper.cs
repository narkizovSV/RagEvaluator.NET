namespace RagEvaluator.Contracts.Interfaces;

/// <summary>
/// Преобразует исходную шкалу релевантности в используемые в системе производные шкалы.
/// </summary>
public interface IRelevanceScaleMapper
{
    /// <summary>
    /// Маппит исходный балл релевантности в бинарную и тернарную шкалы.
    /// </summary>
    /// <param name="sourceScore">Исходная оценка релевантности из датасета.</param>
    /// <returns>Набор значений для бинарной и тернарной шкал.</returns>
    (int Binary, int Ternary) Map(int sourceScore);
}
