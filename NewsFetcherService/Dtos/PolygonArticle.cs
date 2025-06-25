namespace NewsFetcherService.Models;

public class PolygonArticle
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Author { get; set; }
    public string Article_Url { get; set; } = null!;
    public string? Image_Url { get; set; }
    public List<string> Tickers { get; set; } = new();
    public DateTime Published_Utc { get; set; }
    public PolygonPublisher? Publisher { get; set; }
    public List<PolygonInsight>? Insights { get; set; }
}
