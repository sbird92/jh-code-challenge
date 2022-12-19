using SCB.TwitterAnalyzer.Services.Tweets;
using System.Runtime.CompilerServices;

namespace SCB.TwitterAnalyzer.Services.Tests;

public class SampleStreamClientTestFixture : ISampleStreamClient, IDisposable
{
    private string[]? _tweetData;
    public void Dispose()
    {
        _tweetData = null;
    }

    public void LoadTestData(string fileName)
    {
        var path = Path.Combine(Environment.CurrentDirectory, "TestData", fileName);
        var lines = File.ReadAllLines(path);

        if (lines.Any())
            _tweetData = lines ?? throw new InvalidOperationException("The test data file had no data");

    }

    public async IAsyncEnumerable<string> GetTweetsSampleStream([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (_tweetData == null)
            throw new InvalidOperationException("There is no data loaded for testing");

        foreach (var tweet in _tweetData)
        {
            if (cancellationToken.IsCancellationRequested) continue;
            yield return tweet;
        }

        await Task.CompletedTask;
    }
}
