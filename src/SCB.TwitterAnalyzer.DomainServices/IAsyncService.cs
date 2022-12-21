namespace SCB.TwitterAnalyzer.Domain.Services;

public interface IAsyncService
{
    Task StartAsync();
    Task StopAsync();
}