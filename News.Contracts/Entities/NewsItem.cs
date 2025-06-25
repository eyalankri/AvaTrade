using Common;
using System.ComponentModel.DataAnnotations;

namespace News.Contracts.Entities
{
    public class NewsItem : IEntity
    {
        [Key]
        public Guid Id { get; set; } // Internal ID
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime PublishedAt { get; set; }

        public string ExternalId { get; set; } = null!;

        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Author { get; set; }
        public string ArticleUrl { get; set; } = null!;
        public string? ImageUrl { get; set; }

        public string? Ticker { get; set; }
        public string? PublisherName { get; set; }
        public string? PublisherUrl { get; set; }

        public string? Sentiment { get; set; }
        public string? SentimentReasoning { get; set; }



        public string? RawJson { get; set; }
    }
}
