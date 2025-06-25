namespace NewsApi.Dtos
{
    public class NewsArticle
    {
        public string Id { get; set; } = null!;
        public Publisher Publisher { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public DateTime PublishedUtc { get; set; }
        public string ArticleUrl { get; set; } = null!;
        public List<string> Tickers { get; set; } = new();
        public string ImageUrl { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<string> Keywords { get; set; } = new();
        public List<Insight> Insights { get; set; } = new();
    }
}
