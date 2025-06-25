using System.Net.Http.Json;
using Common.Repositories;
using NewsFetcherService.Extensions;
using NewsFetcherService.Models;
using News.Contracts.Entities;


public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public Worker(
        ILogger<Worker> logger,
        IServiceProvider serviceProvider,
        IConfiguration config,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _config = config;
        _httpClient = httpClientFactory.CreateClient();
        _apiKey = _config["PolygonApiKey"]!;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Fetching news at: {time}", DateTimeOffset.Now);

            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<NewsItem>>();

            var url = $"https://api.polygon.io/v2/reference/news?limit=15&apiKey={_apiKey}";
            var result = await _httpClient.GetFromJsonAsync<PolygonNewsResponse>(url, cancellationToken: stoppingToken);

            if (result?.Results is not null)
            {
                foreach (var article in result.Results)
                {
                    if (await repository.GetAsync(x => x.ExternalId == article.Id) is null)
                    {
                        var news = article.ToNewsItem();
                        await repository.CreateAsync(news);
                        _logger.LogInformation("Stored: {title}", news.Title);
                    }
                    else
                    {
                        _logger.LogInformation("Skipped duplicate: {id}", article.Id);
                    }
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken); // Run every hour
        }
    }
}
