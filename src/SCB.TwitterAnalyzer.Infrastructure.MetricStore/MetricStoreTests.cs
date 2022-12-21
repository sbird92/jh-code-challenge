using System.Collections.Concurrent;

namespace SCB.TwitterAnalyzer.Infrastructure.MetricStore.Tests;

public class MetricStoreTests
{
    private readonly MetricsStore.MetricStore _sut;
    private readonly ConcurrentDictionary<string, long> _metricStore = new();    
    public MetricStoreTests()
    {
        _sut = new MetricsStore.MetricStore(_metricStore);
    }
    
    [Fact]
    public void AddOrIncrementAddOrIncrementMetric_add_new_metric_adds_key_with_value_one()
    {
        var key = "test";
        _sut.AddOrIncrementMetric(key);
        _metricStore[key].Should().Be(1);
    }

    [Fact]
    public void AddOrIncrementAddOrIncrementMetric_given_the_key_exists_then_the_metric_is_incremented_by_one()
    {
        _metricStore.TryAdd("test", 1);
        var key = "test";
        _sut.AddOrIncrementMetric(key);
        _metricStore[key].Should().Be(2);
    }

    [Fact]
    public void GetMetric_given_the_key_exists_then_the_value_is_returned()
    {
        _metricStore.TryAdd("test", 2);
        var key = "test";
        _sut.GetMetric(key).Should().Be(2);
    }

    [Fact]
    public void GetMetrics_given_the_mulitple_matching_keys_the_dictionary_of_keys_is_returned()
    {
        var key = "testy";
        _metricStore.TryAdd("testy:A", 2);
        _metricStore.TryAdd("testy:B", 3);
        _metricStore.TryAdd("testy:C", 4);
        _metricStore.TryAdd("not-returned", 5);
        
        _sut.GetMetrics(key).Should()
            .BeEquivalentTo( new Dictionary<string, long>()
            {
                { "testy:A", 2 },
                { "testy:B", 3 },
                { "testy:C", 4 }
            });
    }

    [Fact]
    public void GetMetrics_get_all_dictionary_entries()
    {
        _metricStore.TryAdd("testy:A", 2);
        _metricStore.TryAdd("testy:B", 3);
        _metricStore.TryAdd("testy:C", 4);
        _metricStore.TryAdd("returned", 5);

        _sut.GetMetrics().Should()
            .BeEquivalentTo(new Dictionary<string, long>()
            {
                { "testy:A", 2 },
                { "testy:B", 3 },
                { "testy:C", 4 },
                { "returned", 5 }
            });
    }
}