
namespace SCB.TwitterAnalyzer.Domain.Models;

public class Metric<TData>
{
    public string Key { get; set; } = string.Empty;
    public TData? Value { get; set; }
}
