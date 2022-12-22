
using System.Runtime.CompilerServices;

namespace SCB.TwitterAnalyzer.Infrastructure.Queue.Tests;

public class TweetQueueClientTests
{
    private readonly TweetQueueClient _sut = new TweetQueueClient(new Mock<ILogger<TweetQueueClient>>().Object);

    [Fact]
    public async Task  Enqueue_when_tweet_is_enqueued_the_tweet_enqueued_event_is_raised()
    {
        _sut.TweetEnqueued += HandleTweetEnqueued;
        await _sut.EnqueueAsync(new Tweet());
        _eventRaised.Should().BeTrue();
    }

    private bool _eventRaised = false;
    private async Task HandleTweetEnqueued(object? sender, EventArgs args)
    {
        await Task.Run(() => { _eventRaised = true; });
    }
}