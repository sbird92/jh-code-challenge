using SCB.TwitterAnalyzer.Domain.Models;

namespace SCB.TwitterAnalyzer.Domain.Services;

public interface IMetricService
{
    Metrics GetMetrics();      
}
