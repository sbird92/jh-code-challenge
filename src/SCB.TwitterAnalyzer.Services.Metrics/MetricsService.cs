using SCB.TwitterAnalyzer.Domain.Models;
using SCB.TwitterAnalyzer.Domain.Services;

namespace SCB.TwitterAnalyzer.Services.Metrics;

public class MetricsService : IMetricService
{
    private readonly IMetricStore _metricStore;

    public MetricsService(IMetricStore metricStore)
    {
        _metricStore = metricStore;
    }

    public Domain.Models.Metrics GetMetrics()
    {
        var metrics = new Domain.Models.Metrics();

        var rawData = _metricStore.GetMetrics();

        metrics.TotalTweets = rawData["total-tweets"];
        metrics.HashTags = GetTopCategory(rawData, "hashtag", 10, 2);
        metrics.DistinctTags = GetDistinctCount(rawData, "hashtag");
        metrics.Languages = GetTopCategory(rawData, "language", 10, 2);
        metrics.DistinctLanguages = GetDistinctCount(rawData, "language");
        return metrics;
    }

    private long GetDistinctCount(Dictionary<string, long> rawData, string categery)
    {
        return rawData.Where(kvp => kvp.Key.StartsWith(categery))
            .Count();
    }

    private IEnumerable<Metric> GetTopCategory(Dictionary<string,long> rawData, string categery, int count, int minValue)
    {
        return rawData
            .Where(kvp => kvp.Key.StartsWith(categery) && kvp.Value >= minValue)
            .OrderByDescending(kvp => kvp.Value)
            .Take(count)
            .Select(kvp => new Metric() { Key = kvp.Key, Value = kvp.Value });
    }
}
