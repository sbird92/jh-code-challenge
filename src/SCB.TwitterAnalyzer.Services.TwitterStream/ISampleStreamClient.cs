using System.Runtime.CompilerServices;

namespace SCB.TwitterAnalyzer.Services.Tweets;

public interface ISampleStreamClient
{
    IAsyncEnumerable<string> GetTweetsSampleStream(CancellationToken cancellationToken);
}
