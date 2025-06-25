using Microsoft.EntityFrameworkCore;
using News.Contracts.Entities;

namespace NewsFetcherService.Persistence;

public class NewsDbContext : DbContext
{
    public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options) { }

    public DbSet<NewsItem> News => Set<NewsItem>();
 
}
