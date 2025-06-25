using Microsoft.EntityFrameworkCore;
using News.Contracts.Entities;

namespace NewsApi.Persistence
{
    public class NewsDbContext : DbContext
    {
        public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options) { }

        public DbSet<NewsItem> News => Set<NewsItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsItem>(entity =>
            {
                entity.HasIndex(n => n.ExternalId).IsUnique();
                entity.HasIndex(n => n.PublishedAt);
                entity.HasIndex(n => n.Ticker);
                entity.HasIndex(n => n.Title);
                entity.HasIndex(n => n.Description);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
