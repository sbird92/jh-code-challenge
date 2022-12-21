
using SCB.TwitterAnalyzer.Domain.Models;
using SCB.TwitterAnalyzer.Domain.Services;
using SCB.TwitterAnalyzer.Services.Metrics;

namespace SCB.TwitterAnalyzer.Services.Tests;

public class HashTagCountUpdaterTests
{
    private readonly Mock<IMetricStore> _metricStoreMock = new();
    private readonly HashTagCountUpdater _sut;
    public HashTagCountUpdaterTests()
    {
        _sut = new HashTagCountUpdater(_metricStoreMock.Object, new Mock<ILogger<MetricUpdater>>().Object);
    }

    [Fact]
    public void UpdateMetrics_given_the_tweet_has_hashtags_a_metric_should_counted_for_each()
    {
        var tweet = new Tweet()
        {
            Entities = new()
            {
                HashTags = new[]
                {
                    new HashTag() { Tag = "gunter" },
                    new HashTag() { Tag = "gleiben" },
                    new HashTag() { Tag = "glauchen" },
                    new HashTag() { Tag = "globen" }
                }
            }
        };

        _sut.UpdateMetrics(tweet);

        _metricStoreMock.Verify(m => m.AddOrIncrementMetric("hashtag:gunter"), Times.Once());
        _metricStoreMock.Verify(m => m.AddOrIncrementMetric("hashtag:gleiben"), Times.Once());
        _metricStoreMock.Verify(m => m.AddOrIncrementMetric("hashtag:glauchen"), Times.Once());
        _metricStoreMock.Verify(m => m.AddOrIncrementMetric("hashtag:globen"), Times.Once());
    }

    [Fact]
    public void UpdateMetrics_given_the_tweet_has_no_hashtags_a_metrics_should_not_be_counted()
    {
        var tweet = new Tweet();

        _sut.UpdateMetrics(tweet);

        _metricStoreMock.Verify(m => m.AddOrIncrementMetric(It.IsAny<string>()), Times.Never);
    }
}
