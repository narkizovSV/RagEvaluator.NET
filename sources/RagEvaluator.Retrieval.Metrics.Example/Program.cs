using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RagEvaluator.Retrieval.Metrics.Interfaces;
using RagEvaluator.Retrieval.Metrics.IoC;
using RagEvaluator.Retrieval.Metrics.Models.Configurations;
using RagEvaluator.Utilities;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .Build();

var services = new ServiceCollection();

services.AddLogging(builder => builder.AddConsole());
services.AddRetrievalMetrics(config);
services.PostConfigure<MetricSettings>(options =>
{
    var baseDirectory = AppContext.BaseDirectory;
    options.QrelsFilePath = Path.GetFullPath(Path.Combine(baseDirectory, options.QrelsFilePath));
    options.RunFilePath = Path.GetFullPath(Path.Combine(baseDirectory, options.RunFilePath));
    options.OutputFilePath = Path.GetFullPath(Path.Combine(baseDirectory, options.OutputFilePath));
});

await using var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();

var metricSettings = scope.ServiceProvider.GetRequiredService<IOptions<MetricSettings>>().Value;
var evaluator = scope.ServiceProvider.GetRequiredService<IRetrievalMetricsEvaluator>();

var summary = await evaluator.Evaluate();

await JsonFileObjectStorage.WriteAsync(
    metricSettings.OutputFilePath,
    summary);

var json = JsonSerializer.Serialize(summary, JsonFileObjectStorage.FileStorage);
Console.WriteLine(json);
