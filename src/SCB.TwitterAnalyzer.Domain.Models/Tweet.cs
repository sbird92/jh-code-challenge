namespace SCB.TwitterAnalyzer.Domain.Models
{
    public record Tweet
    {
        public string Id { get; init; } = string.Empty;
        public string Text { get; init; } = string.Empty;
        public DateTime? CreatedAt { get; init; } 
        public IEnumerable<string> EditHistoryTweetIds { get; init; } = Enumerable.Empty<string>();
        public Entities? Entities { get; init; }
        public string Language { get; init; } = string.Empty;
        public string Source { get; init; } = string.Empty;
        public bool PossiblySensitive { get; init; }
    }
}