using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SCB.TwitterAnalyzer.Domain.Services;
using SCB.TwitterAnalyzer.Infrastructure.App;
using System.Diagnostics;
using System.Net;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(o =>
    {
        o.SetBasePath(Directory.GetCurrentDirectory());
        o.AddJsonFile("appsettings.json", false, false);
    })
    .ConfigureLogging((c, o) =>
    {
        o.AddConfiguration(c.Configuration);
    })
    .ConfigureServices((c, s) => s.ConfigureServices(c.Configuration))
    .Build();


var services = host.Services;

var logger = services.GetRequiredService<ILogger<Program>>();
var source = new CancellationTokenSource();

logger.LogTrace($"--STARTING PRORGAM--");
var backgroundStream = services.GetRequiredService<IAsyncService>();
var listener = services.GetRequiredService<ITweetMetricListener>();
var writer = services.GetRequiredService<ConsoleMetricWriter>();

listener.StartListeningForTweets();
_ = backgroundStream.StartAsync();
writer.Start();

Console.WriteLine("Press Esc to end the program");
while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)) { }

writer.Stop();
await backgroundStream.StopAsync();
listener.StopListeningForTweets();

Console.WriteLine("--COMPLETE--");