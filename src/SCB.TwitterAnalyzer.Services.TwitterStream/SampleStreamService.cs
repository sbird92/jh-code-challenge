using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SCB.TwitterAnalyzer.Domain.Models;
using SCB.TwitterAnalyzer.Domain.Services;
using SCB.TwitterAnalyzer.Services.Tweets;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SCB.TwitterAnalyzer.Services.TwitterStream;

public class SampleStreamService :  IAsyncService
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

    Task? _executeTask;
    public Task StartAsync()
    {
        _executeTask = GetTweetSampleStreamAsync(_source.Token);
        return _executeTask ?? Task.CompletedTask;
    }

    public async Task StopAsync()
    {
        _source.Cancel();
        if (_executeTask != null)
            await _executeTask;
    }

    private async Task GetTweetSampleStreamAsync(CancellationToken token)
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
            await StopAsync();
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
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Deserialization Error: {json}", json);
            return false;
        }
    }

}