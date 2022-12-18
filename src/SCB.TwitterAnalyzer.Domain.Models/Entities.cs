
using System.Text.Json.Serialization;

namespace SCB.TwitterAnalyzer.Domain.Models;

public record Entities
{
    [JsonPropertyName("annotations")]
    public IEnumerable<Anotation> Anotations { get; init; } = Enumerable.Empty<Anotation>();
    [JsonPropertyName("urls")]
    public IEnumerable<Url> Urls { get; init; } = Enumerable.Empty<Url>();
    [JsonPropertyName("hashtags")]
    public IEnumerable<HashTag> HashTags { get; init; } = Enumerable.Empty<HashTag>();
    [JsonPropertyName("mentions")]
    public IEnumerable<Mention> Mentions { get; init; } = Enumerable.Empty<Mention>();

}

public abstract record Entity
{
    [JsonPropertyName("start")]
    public int Start { get; init; }
    [JsonPropertyName("end")]
    public int End { get; init; }
}

public record Anotation : Entity
{
    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;
    [JsonPropertyName("normalized_text")]
    public string NormalizedText { get;init; } = string.Empty;

}

public record Url : Entity
{
    [JsonPropertyName("url")]
    public string UserUrl { get; init; } = string.Empty;
    [JsonPropertyName("expanded_url")]
    public string ExpandedUrl { get; init; } = string.Empty;
    [JsonPropertyName("display_url")]
    public string DisplayUrl { get; init; } = string.Empty;
    [JsonPropertyName("unwound_url")]
    public string UnwoundUrl { get; init; } = string.Empty;
}

public record HashTag : Entity
{
    [JsonPropertyName("tag")]
    public string Tag { get; init; } = string.Empty;
}

public record Mention : Entity
{
    [JsonPropertyName("username")]
    public string UserName { get; init; } = string.Empty;
}

public record CashTag : Entity
{
    [JsonPropertyName("tag")]
    public string Tag { get; init; } = string.Empty;
}
