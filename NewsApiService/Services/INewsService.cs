using News.Contracts.Entities;


namespace NewsApi.Services
{
    public interface INewsService
    {
        Task<IEnumerable<NewsItem>> GetAllAsync();
        Task<NewsItem?> GetByIdAsync(Guid id);
        Task<IEnumerable<NewsItem>> GetByTickerAsync(string ticker, int limit = 10);
        Task<IEnumerable<NewsItem>> GetByDateRangeAsync(int days);
        Task<IEnumerable<NewsItem>> SearchByTextAsync(string text);
        Task AddNewsAsync(NewsItem news);
        Task<IEnumerable<NewsItem>> GetTopLatestNewsByInstrumentAsync(int count);

    }
}
