using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SCB.TwitterAnalyzer.Domain.Models;
using SCB.TwitterAnalyzer.Domain.Services;

namespace SCB.TwitterAnalyzer.Services.Metrics;

public class TweetMetricProcessor : ITweetMetricListener
{
    private readonly ITweetQueue _tweetQueue;
    private readonly IEnumerable<MetricUpdater> _updaters;
    private readonly ILogger<TweetMetricProcessor> _logger;

    public TweetMetricProcessor(ITweetQueue tweetQueue,
        IEnumerable<MetricUpdater> updaters,
        ILogger<TweetMetricProcessor> logger)
    {
        _tweetQueue = tweetQueue;
        _updaters = updaters;
        _logger = logger;
    }

    public void StartListeningForTweets()
    {
        _logger.LogTrace("Start listening for tweets");
        _tweetQueue.TweetEnqueued += HandleTweetEnqueued;
    }

    public void StopListeningForTweets()
    {
        _logger.LogTrace("Stop listening for tweets");
        _tweetQueue.TweetEnqueued -= HandleTweetEnqueued;
    }


    private async Task HandleTweetEnqueued(object? sender, EventArgs args)
    {
        if(_tweetQueue.TryDequeue(out var tweet))
        {
            await Task.Run(() => ProcessMetricUpdates(tweet));
            return;
        }
        _logger.LogWarning("No Tweet was removed fromt he tweet queue");
    }

    private void ProcessMetricUpdates(Tweet? tweet)
    {
        if (tweet != null)
        {
            foreach (var updater in _updaters)
            {
                updater.UpdateMetrics(tweet);
            }
            return;
        }
        _logger.LogWarning("Cannot process updates for null tweet");
    }

}