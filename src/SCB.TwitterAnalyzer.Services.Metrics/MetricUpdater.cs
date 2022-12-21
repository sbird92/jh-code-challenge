using Microsoft.Extensions.Logging;
using SCB.TwitterAnalyzer.Domain.Models;
using SCB.TwitterAnalyzer.Domain.Services;
using System.Text.Json;

namespace SCB.TwitterAnalyzer.Services.Metrics;

public abstract class MetricUpdater
{
    private readonly IMetricStore _metricStore;
    private readonly ILogger<MetricUpdater> _logger;

    public MetricUpdater(IMetricStore metricStore, ILogger<MetricUpdater> logger)
    {
        _metricStore = metricStore;
        _logger = logger;
    }

    public virtual void UpdateMetrics(Tweet tweet)
    {
        if (CanIncrementCount(tweet))
            foreach (var key in GetKeys(tweet)) { _metricStore.AddOrIncrementMetric(key); }

        _logger.LogTrace(JsonSerializer.Serialize(_metricStore.GetMetrics()));
    }

    protected abstract IEnumerable<string> GetKeys(Tweet tweet);

    protected abstract bool CanIncrementCount(Tweet? tweet);
}
