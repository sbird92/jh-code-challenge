using SCB.TwitterAnalyzer.Domain.Models;

namespace SCB.TwitterAnalyzer.Domain.Services
{
    public interface ITweetQueue
    {
        event EventHandler TweetEnqueued;
        void Enqueue(Tweet tweet);
    }
}
