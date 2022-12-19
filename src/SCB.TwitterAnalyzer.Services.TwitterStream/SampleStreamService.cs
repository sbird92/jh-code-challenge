using Microsoft.Extensions.Logging;
using SCB.TwitterAnalyzer.Domain.Models;
using SCB.TwitterAnalyzer.Domain.Services;
using SCB.TwitterAnalyzer.Services.Tweets;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace SCB.TwitterAnalyzer.Services.TwitterStream;

public class SampleStreamService : IBackgroundService
{
    private readonly ISampleStreamClient _tweetStreamClient;
    private readonly ITweetQueue _tweetQueue;
    private readonly ILogger<SampleStreamService> _logger;
    private readonly CancellationTokenSource _source = new();
    public List<Tweet> Tweets { get; set; } = new();

    public SampleStreamService(ISampleStreamClient tweetStreamClient, ITweetQueue tweetQueue, ILogger<SampleStreamService> logger)
    {
        _tweetStreamClient = tweetStreamClient;
        _tweetQueue = tweetQueue;
        _logger = logger;
    }

    Task? _streamTask;
    public Task StartAsync()
    {
        _logger.LogTrace($"Start {nameof(SampleStreamService)}");
        var token = _source.Token;
        _streamTask = GetTweetSampleStream(token);
        return _streamTask;
    }

    private async Task GetTweetSampleStream(CancellationToken token)
    {
        try
        {
            var enumerator = _tweetStreamClient.GetTweetsSampleStream(token).GetAsyncEnumerator(token);
            while (await enumerator.MoveNextAsync() && !token.IsCancellationRequested)
            {
                var tweetString = enumerator.Current;

                var tweet = new Tweet();
                if (TryDeserializeTweet(tweetString, out tweet))
                    _tweetQueue.Enqueue(tweet);
            }
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "An error occurred while reading the tweet stream");
        }
    }

    private bool TryDeserializeTweet(string json, out Tweet tweet)
    {
        tweet = new Tweet();
        try
        {
            var jsonObj = JsonObject.Parse(json);
            var data = jsonObj != null ? jsonObj["data"] : throw new InvalidOperationException($"Could not parse Json object: {json}");
            
            if (data == null)
                throw new InvalidOperationException($"There is no data node present to deserialize: {json}"); 

            tweet = JsonSerializer.Deserialize<Tweet>(data) ?? throw new InvalidOperationException($"Data was not deserializable to a Tweet object: {json}");
            return tweet != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Deserialization Error: {json}", json);
            return false;
        }
    }

    public async Task StopAsync()
    {
        var name = nameof(SampleStreamService);
        
        _logger.LogTrace("Stop {name}",name);
        _source.Cancel();

        if (_streamTask != null)
            await _streamTask;
    }
}