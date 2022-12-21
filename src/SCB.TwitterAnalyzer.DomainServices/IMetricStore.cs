namespace SCB.TwitterAnalyzer.Domain.Services;

public interface IMetricStore
{
    void AddOrIncrementMetric(string key);
    Dictionary<string, long> GetMetrics(string key);
    Dictionary<string, long> GetMetrics();
}
