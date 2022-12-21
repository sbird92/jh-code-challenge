
namespace SCB.TwitterAnalyzer.Domain.Services;

public interface ITweetMetricListener
{
    void StartListeningForTweets();
    void StopListeningForTweets();
}
