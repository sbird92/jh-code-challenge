
using SCB.TwitterAnalyzer.Domain.Models;
using SCB.TwitterAnalyzer.Domain.Services;
using SCB.TwitterAnalyzer.Services.Metrics;

namespace SCB.TwitterAnalyzer.Services.Tests;

public class TweetCountUpdaterTests
{
    private readonly Mock<IMetricStore> _metricStoreMock = new();
    private readonly TweetCountUpdater _sut;

    public TweetCountUpdaterTests()
    {
        _sut = new TweetCountUpdater(_metricStoreMock.Object, new Mock<ILogger<MetricUpdater>>().Object);
    }

    [Fact]
    public void UpdateMetric_given_a_tweet_the_AddOrIncrementMetric_is_called_once()
    {
        var tweet = new Tweet();
        _sut.UpdateMetrics(tweet);

        _metricStoreMock.Verify(m => m.AddOrIncrementMetric(It.Is<string>(s => s.Equals("total-tweets"))), times: Times.Once);
    }

    [Fact]
    public void UpdateMetric_given_a_null_tweet_the_AddOrIncrementMetric_is_not_called()
    {
        Tweet? tweet = default;
#pragma warning disable CS8604 // Testing the results from the method for a null tweet here.
        _sut.UpdateMetrics(tweet);
#pragma warning restore CS8604 // Possible null reference argument.

        _metricStoreMock.Verify(m => m.AddOrIncrementMetric(It.Is<string>(s => s.Equals("total-tweets"))), times: Times.Never);
    }
}
