using Microsoft.AspNetCore.Mvc;
using NewsApi.Services;

namespace NewsApi.Controllers;

[ApiController]
[Route("api/public")]
public class PublicController : ControllerBase
{
    private readonly INewsService _newsService;

    public PublicController(INewsService newsService)
    {
        _newsService = newsService;
    }

    [HttpGet("latest")]
    public async Task<IActionResult> GetLatest()
    {
        var latest = await _newsService.GetTopLatestNewsByInstrumentAsync(5);  
        return Ok(latest);
    }
}
