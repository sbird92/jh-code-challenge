using Microsoft.Extensions.Logging;
using SCB.TwitterAnalyzer.Services.Tweets;
using System.Runtime.CompilerServices;

namespace SCB.TwitterAnalyzer.Infrastructure;

public class SampleStreamClient : ISampleStreamClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SampleStreamClient> _logger;

    public SampleStreamClient(HttpClient httpClient, ILogger<SampleStreamClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async IAsyncEnumerable<string> GetTweetsSampleStream([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        _logger.LogTrace("BEGIN - TWEET STREAM");
        var response = await _httpClient.GetAsync("tweets/sample/stream?tweet.fields=edit_history_tweet_ids,entities,id,lang,source,text,created_at", 
            HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        using var reader = new StreamReader(stream);
        while (!reader.EndOfStream)
        {
            var data = await reader.ReadLineAsync();
            yield return data ?? throw new InvalidDataException("Invalid data received in feed. ");
        }

        _logger.LogTrace("END - TWEET STREAM");
    }
}