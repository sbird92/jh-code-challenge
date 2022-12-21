using SCB.TwitterAnalyzer.Domain.Services;
using System.Collections.Concurrent;

namespace SCB.TwitterAnalyzer.Infrastructure.MetricsStore
{
    public class MetricStore : IMetricStore
    {
        private readonly ConcurrentDictionary<string, long> _metrics;

        public MetricStore(ConcurrentDictionary<string, long>? metrics)
        {
            _metrics = metrics ?? new ConcurrentDictionary<string, long>(); 
        }

        private readonly object _lock = new();
        public void AddOrIncrementMetric(string key)
        {
            ExecuteSafe(m =>  m.AddOrUpdate(key, 1, (k, v) => v += 1));
        }

        public long GetMetric(string key)
        {
            return ExecuteSafe(m =>
            {
                m.TryGetValue(key, out long value);
                return value;
            });
        }

        public Dictionary<string,long> GetMetrics(string key)
        {
            return ExecuteSafe(m =>
            {
                return m.Where(kvp => kvp.Key.Contains($"{key}:"))
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            });
        }

        public Dictionary<string, long> GetMetrics()
        {
            return ExecuteSafe(m => m.ToDictionary(kvp => kvp.Key,
                                                   kvp => kvp.Value));
        }

        private TData ExecuteSafe<TData>(Func<ConcurrentDictionary<string, long>,TData> func)
        {
            lock(_lock)
            {
                return func(_metrics);
            }
        }
    }
}