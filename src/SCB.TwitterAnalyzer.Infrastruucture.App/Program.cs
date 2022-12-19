using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SCB.TwitterAnalyzer.Domain.Services;
using SCB.TwitterAnalyzer.Infrastructure.App;


var serviceProvider = ServiceConfiguration.ConfigureServiceProvider(new ServiceCollection());

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

logger.LogTrace("Start - Twitter Ananlyzer App");

var sampleStreamService = serviceProvider.GetRequiredService<IBackgroundService>();
sampleStreamService.StartAsync();

while (!Console.KeyAvailable && Console.ReadKey(true).Key != ConsoleKey.Escape) { }

await sampleStreamService.StopAsync();
logger.LogTrace("End Program");

