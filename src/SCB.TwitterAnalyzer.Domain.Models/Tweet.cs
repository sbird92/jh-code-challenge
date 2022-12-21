using System.Text.Json.Serialization;

namespace SCB.TwitterAnalyzer.Domain.Models;

public record Tweet
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;
    
    [JsonPropertyName("text")]
    public string Text { get; init; } = string.Empty;
    
    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; init; }

    [JsonPropertyName("edit_history_tweet_ids")]
    public IEnumerable<string> EditHistoryTweetIds { get; init; } = Enumerable.Empty<string>();
    
    [JsonPropertyName("entities")]
    public Entities Entities { get; init; } = new Entities();
    
    [JsonPropertyName("lang")]
    public string Language { get; init; } = string.Empty;
    
    [JsonPropertyName("source")]
    public string Source { get; init; } = string.Empty;
}