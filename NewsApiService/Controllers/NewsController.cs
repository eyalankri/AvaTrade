using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NewsApi.Services;

namespace NewsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NewsController : ControllerBase
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var news = await _newsService.GetAllAsync();
        return Ok(news);
    }

    [HttpGet("range/{days:int}")]
    public async Task<IActionResult> GetByDateRange(int days)
    {
        var news = await _newsService.GetByDateRangeAsync(days);
        return Ok(news);
    }

    [HttpGet("instrument/{ticker}")]
    public async Task<IActionResult> GetByInstrument(string ticker, [FromQuery] int limit = 10)
    {
        var news = await _newsService.GetByTickerAsync(ticker, limit);
        return Ok(news);
    }

    [HttpGet("search/{text}")]
    public async Task<IActionResult> SearchByText(string text)
    {
        var news = await _newsService.SearchByTextAsync(text);
        return Ok(news);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var newsItem = await _newsService.GetByIdAsync(id);
        if (newsItem == null)
            return NotFound();

        return Ok(newsItem);
    }
}
