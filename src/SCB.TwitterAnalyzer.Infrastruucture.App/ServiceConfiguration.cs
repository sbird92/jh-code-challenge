using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SCB.TwitterAnalyzer.Domain.Services;
using SCB.TwitterAnalyzer.Infrastructure.Queue;
using SCB.TwitterAnalyzer.Services.Tweets;
using SCB.TwitterAnalyzer.Services.TwitterStream;

namespace SCB.TwitterAnalyzer.Infrastructure.App
{
    internal static class ServiceConfiguration
    {
        public static IServiceProvider ConfigureServiceProvider(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();

            services.AddLogging(c =>
            {
                c.ClearProviders();
                c.AddConsole()
                    .AddFilter("SCB.TwitterAnalyzer", LogLevel.Trace)
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System.Net.Http.HttpClient", LogLevel.Warning);
            });

            services.AddHttpClient<ISampleStreamClient, SampleStreamClient>(client =>
            {
                var httpClientConfig = configuration.GetSection("HttpClientConfig");
                var uriString = httpClientConfig["BaseUrl"];
                var authToken = httpClientConfig["AuthToken"];

                Uri uri = new(uriString ?? throw new InvalidOperationException("Configure the uri in appsettings"));
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            });

            services.AddSingleton<ITweetQueue, TweetQueueClient>();
            services.AddSingleton<IBackgroundService, SampleStreamService>();
            
            return services.BuildServiceProvider();
        }
    }
}
