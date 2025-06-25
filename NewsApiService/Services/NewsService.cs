using Common.Repositories;
using News.Contracts.Entities;

namespace NewsApi.Services
{
    public class NewsService : INewsService
    {
        private readonly IRepository<NewsItem> _repository;

        public NewsService(IRepository<NewsItem> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<NewsItem>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<NewsItem?> GetByIdAsync(Guid id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<IEnumerable<NewsItem>> GetByTickerAsync(string ticker, int limit = 10)
        {
            var items = await _repository.GetAllAsync(x => x.Ticker != null && x.Ticker.ToLower() == ticker.ToLower());
            return items.OrderByDescending(x => x.PublishedAt).Take(limit);
        }

        public async Task<IEnumerable<NewsItem>> GetByDateRangeAsync(int days)
        {
            var fromDate = DateTime.UtcNow.AddDays(-days);
            return await _repository.GetAllAsync(x => x.PublishedAt >= fromDate);
        }

        public async Task<IEnumerable<NewsItem>> SearchByTextAsync(string text)
        {
            text = text.ToLower();
            return await _repository.GetAllAsync(x =>
                (x.Title != null && x.Title.ToLower().Contains(text)) ||
                (x.Description != null && x.Description.ToLower().Contains(text)) ||
                (x.Ticker != null && x.Ticker.ToLower().Contains(text)));
        }

        public async Task AddNewsAsync(NewsItem news)
        {
            await _repository.CreateAsync(news);
        }

        public async Task<IEnumerable<NewsItem>> GetTopLatestNewsByInstrumentAsync(int count)
        {
            var allNews = await _repository.GetAllAsync();

            var topPerInstrument = allNews
                .Where(n => !string.IsNullOrEmpty(n.Ticker))
                .GroupBy(n => n.Ticker!.ToLower())
                .Select(g => g.OrderByDescending(n => n.PublishedAt).First())
                .OrderByDescending(n => n.PublishedAt)
                .Take(count);

            return topPerInstrument;
        }

    }
}
