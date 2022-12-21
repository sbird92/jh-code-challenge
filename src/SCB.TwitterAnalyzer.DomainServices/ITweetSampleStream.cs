
namespace SCB.TwitterAnalyzer.Domain.Services;


public interface ITweetSampleStream
{
    Task GetTweetSampleStreamAsync(CancellationToken token);
}
