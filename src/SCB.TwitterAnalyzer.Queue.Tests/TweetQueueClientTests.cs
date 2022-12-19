
namespace SCB.TwitterAnalyzer.Infrastructure.Queue.Tests;

public class TweetQueueClientTests
{
    private readonly TweetQueueClient _sut = new TweetQueueClient(new Mock<ILogger<TweetQueueClient>>().Object);

    [Fact]
    public void Enqueue_when_tweet_is_enqueued_the_tweet_enqueued_event_is_raised()
    {
        _sut.TweetEnqueued += HandleTweetEnqueued;
        _sut.Enqueue(new Tweet());
        _eventRaised.Should().BeTrue();
    }

    private bool _eventRaised = false;
    private void HandleTweetEnqueued(object? sender, EventArgs args)
    {
        _eventRaised = true;
    }
}