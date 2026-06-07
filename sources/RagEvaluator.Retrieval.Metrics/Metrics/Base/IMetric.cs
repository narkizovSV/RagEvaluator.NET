using RagEvaluator.Retrieval.Metrics.Models;

namespace RagEvaluator.Retrieval.Metrics.Base;


public interface IMetric<TContext>
{
    string Name { get; }

    EvaluationResult Evaluate(TContext context);
}
