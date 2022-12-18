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
    private readonly ILogger<SampleStreamService> _logger;
    private readonly CancellationTokenSource _source = new();
    public List<Tweet> Tweets { get; set; } = new();

    public SampleStreamService(ISampleStreamClient tweetStreamClient, ILogger<SampleStreamService> logger)
    {
        _tweetStreamClient = tweetStreamClient;
        _logger = logger;
    }

    Task? _streamTask;
    public void Start()
    {
        _logger.LogTrace($"Start {nameof(SampleStreamService)}");
        var token = _source.Token;
        _streamTask = GetTweetSampleStream(token);
    }

    private async Task GetTweetSampleStream(CancellationToken token)
    {
        try
        {
            var enumerator = _tweetStreamClient.GetTweetsSampleStream(token).GetAsyncEnumerator(token);
            while (await enumerator.MoveNextAsync() && !token.IsCancellationRequested)
            {
                var tweetString = enumerator.Current;
                _logger.LogTrace("{tweetString}", tweetString);
                var twit = DeserializeTweet(tweetString);
            }
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "An error occurred while reading the tweet stream");
        }
    }

    private Tweet? DeserializeTweet(string json)
    {
        try
        {
            var jsonObj = JsonObject.Parse(json);
            if (jsonObj != null)
                return jsonObj["data"].Deserialize<Tweet>();
        }
        catch (Exception ex)
        {
            _logger.LogError("Deserialization Error: {json}", json);
        }
        return null;
    }

    public void Stop()
    {
        var name = nameof(SampleStreamService);
        _logger.LogTrace("Stop {name}",name);
        _source.Cancel();
    }
}