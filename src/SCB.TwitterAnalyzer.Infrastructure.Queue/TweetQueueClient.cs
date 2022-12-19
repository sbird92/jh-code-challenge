using Microsoft.Extensions.Logging;
using SCB.TwitterAnalyzer.Domain.Models;
using SCB.TwitterAnalyzer.Domain.Services;
using System.Collections.Concurrent;

namespace SCB.TwitterAnalyzer.Infrastructure.Queue;

public class TweetQueueClient : ITweetQueue
{
    public event EventHandler? TweetEnqueued;
    private readonly ConcurrentQueue<Tweet> _tweetQueue = new();
    private readonly ILogger<TweetQueueClient> _logger;

    public TweetQueueClient(ILogger<TweetQueueClient> logger)
    {
        _logger = logger;
    }

    public void Enqueue(Tweet tweet)
    {
        _logger.LogTrace($"Enquue tweet");
        _tweetQueue.Enqueue(tweet);
        OnTweetEnqueued();
    }

    private void OnTweetEnqueued()
    {
        TweetEnqueued?.Invoke(this, new EventArgs());
    }
}