using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RagEvaluator.Retrieval.Metrics.Interfaces;
using RagEvaluator.Retrieval.Metrics.IoC;
using RagEvaluator.Retrieval.Metrics.Models;
using RagEvaluator.Retrieval.Metrics.Models.Configurations;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .Build();

var services = new ServiceCollection();

services.AddLogging(builder => builder.AddConsole());
services.AddRetrievalMetrics(config);

await using var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();

var metricSettings = scope.ServiceProvider.GetRequiredService<IOptions<MetricSettings>>().Value;
var evaluator = scope.ServiceProvider.GetRequiredService<IRetrievalMetricsEvaluator>();

var context = new EvaluationContext
{
    EvaluationId = "q1",
    RelevantDocumentIds = new Dictionary<string, int>
    {
        ["doc1"] = 1,
        ["doc3"] = 2,
    },
    RankedDocumentIdsByScoreDesc = new Dictionary<string, double>
    {
        ["doc1"] = 0.95,
        ["doc2"] = 0.80,
        ["doc3"] = 0.70,
        ["doc4"] = 0.60,
        ["doc5"] = 0.50,
    },
    K = metricSettings.TopKValues
};

var summary = evaluator.Evaluate(context);

var json = JsonSerializer.Serialize(summary, new JsonSerializerOptions { WriteIndented = true });
Console.WriteLine(json);
