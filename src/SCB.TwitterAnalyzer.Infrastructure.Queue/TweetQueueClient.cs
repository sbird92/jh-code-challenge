using Microsoft.Extensions.Logging;
using SCB.TwitterAnalyzer.Domain.Models;
using SCB.TwitterAnalyzer.Domain.Services;
using System.Collections.Concurrent;

namespace SCB.TwitterAnalyzer.Infrastructure.Queue;

public class TweetQueueClient : ITweetQueue
{
    public event AsyncEventHandler? TweetEnqueued;
    private readonly ConcurrentQueue<Tweet> _tweetQueue = new();
    private readonly ILogger<TweetQueueClient> _logger;

    public TweetQueueClient(ILogger<TweetQueueClient> logger)
    {
        _logger = logger;
    }
    public bool TryDequeue(out Tweet? tweet)
    {
        _logger.LogTrace($"Enquue tweet");
        tweet = default;

        if(_tweetQueue.TryDequeue(out Tweet? dqtweet))
        {
            tweet = dqtweet;
            return true;
        }

        return false;
    }

    public async Task EnqueueAsync(Tweet tweet)
    {
        _logger.LogTrace($"Enquue tweet");
        _tweetQueue.Enqueue(tweet);
        await OnTweetEnqueued();
    }

    private async Task OnTweetEnqueued()
    {
        if (TweetEnqueued != null)
            await TweetEnqueued.Invoke(this, new EventArgs());
    }
}