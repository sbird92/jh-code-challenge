using SCB.TwitterAnalyzer.Domain.Models;

namespace SCB.TwitterAnalyzer.Domain.Services
{
    public delegate Task AsyncEventHandler (object sender, EventArgs args);
    public interface ITweetQueue
    {
        event AsyncEventHandler TweetEnqueued;
        Task EnqueueAsync(Tweet tweet);
        bool TryDequeue(out Tweet? tweet);    
    }
}
