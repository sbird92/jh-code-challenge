namespace SCB.TwitterAnalyzer.Domain.Services;

public interface IBackgroundService
{
    Task StartAsync();
    Task StopAsync();
}