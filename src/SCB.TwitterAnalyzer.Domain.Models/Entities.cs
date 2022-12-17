
namespace SCB.TwitterAnalyzer.Domain.Models
{
    public record Entities
    {
        public IEnumerable<Anotation> Anotations { get; init; } = Enumerable.Empty<Anotation>();
        public IEnumerable<Url> Urls { get; init; } = Enumerable.Empty<Url>();
        public IEnumerable<HashTag> HashTags { get; init; } = Enumerable.Empty<HashTag>();
        public IEnumerable<Mention> Mentions { get; init; } = Enumerable.Empty<Mention>();

    }

    public abstract record Entity
    {
        public int Start { get; init; }
        public int End { get; init; }
    }

    public record Anotation : Entity
    {
        public string Type { get; init; } = string.Empty;
        public string NormalizedText { get;init; } = string.Empty;

    }

    public record Url : Entity
    {
        public string UserUrl { get; init; } = string.Empty;
        public string ExpandedUrl { get; init; } = string.Empty;
        public string DisplayUrl { get; init; } = string.Empty;
        public string UnwoundUrl { get; init; } = string.Empty;
    }

    public record HashTag : Entity
    {
        public string Tag { get; init; }
    }

    public record Mention : Entity
    {
        public string UserName { get; init; } = string.Empty;
    }

    public record CashTag : Entity
    {
        public string Tag { get; init; }
    }
}
