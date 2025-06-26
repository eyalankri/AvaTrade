using News.Contracts.Entities;
using NewsFetcherService.Models;

namespace NewsFetcherService.Extensions;

public static class PolygonArticleExtensions
{
    public static NewsItem ToNewsItem(this PolygonArticle article)
    {
        return new NewsItem
        {
            Id = Guid.NewGuid(),
            ExternalId = article.Id,
            Title = article.Title,
            Description = article.Description,
            Author = article.Author,
            ArticleUrl = article.Article_Url,
            ImageUrl = article.Image_Url,
            Ticker = article.Tickers.FirstOrDefault(),
            PublisherName = article.Publisher?.Name,
            PublisherUrl = article.Publisher?.Homepage_Url,
            Sentiment = article.Insights?.FirstOrDefault()?.Sentiment,
            SentimentReasoning = article.Insights?.FirstOrDefault()?.Sentiment_Reasoning,
            PublishedAt = article.Published_Utc,
            DateCreated = DateTime.UtcNow,
            RawJson = System.Text.Json.JsonSerializer.Serialize(article)
        };
    }
}
