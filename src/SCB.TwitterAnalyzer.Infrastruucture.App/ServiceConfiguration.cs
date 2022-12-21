using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SCB.TwitterAnalyzer.Domain.Services;
using SCB.TwitterAnalyzer.Infrastructure.MetricsStore;
using SCB.TwitterAnalyzer.Infrastructure.Queue;
using SCB.TwitterAnalyzer.Services.Metrics;
using SCB.TwitterAnalyzer.Services.Tweets;
using SCB.TwitterAnalyzer.Services.TwitterStream;
using System.Collections.Concurrent;

namespace SCB.TwitterAnalyzer.Infrastructure.App
{
    internal static class ServiceConfiguration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
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
            services.AddSingleton<IMetricStore, MetricStore>(_ => new MetricStore(new ConcurrentDictionary<string, long>()));
            services.AddScoped<MetricUpdater, TweetCountUpdater>();
            services.AddScoped<MetricUpdater, HashTagCountUpdater>();
            services.AddScoped<MetricUpdater, LanguageCountUpdater>();
            services.AddSingleton<ITweetMetricListener, TweetMetricProcessor>();
            services.AddSingleton<IAsyncService, SampleStreamService>();

            return services;
        }
    }
}
