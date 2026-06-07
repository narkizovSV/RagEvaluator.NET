using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RagEvaluator.ConsoleApp.IoC;
using RagEvaluator.RelevanceJudge.Interfaces;
using RagEvaluator.RelevanceJudge.IoC;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .Build();

var services = new ServiceCollection();

services.AddLogging(builder => builder.AddConsole());
services.ConfigureAIProvider(config);
services.AddRelevanceJudge(config);

await using var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();

var qrelsExportService = scope.ServiceProvider.GetRequiredService<IQrelsExporter>();
await qrelsExportService.ExportAsync(CancellationToken.None);
