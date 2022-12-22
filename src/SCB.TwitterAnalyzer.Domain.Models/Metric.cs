
namespace SCB.TwitterAnalyzer.Domain.Models;

public class Metrics
{
    public long TotalTweets { get; set; } 
    public IEnumerable<Metric> HashTags { get; set; } = new List<Metric>();
    public long DistinctTags { get; set; }
    public IEnumerable<Metric> Languages { get; set; } = new List<Metric>();
    public long DistinctLanguages { get; set; }
}

public class Metric
{
    public string Key { get; set; } = string.Empty;
    public long Value { get; set; }
}
