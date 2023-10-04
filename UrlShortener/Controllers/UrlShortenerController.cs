using BusinessLogicLayer.Services;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlShortenerController : ControllerBase
    {
        private readonly IUrlShortener _urlShortener;
        public UrlShortenerController(IUrlShortener urlShortener)
        {
            _urlShortener = urlShortener;
        }

        [HttpPost("shortUrl")]
        public async Task<IActionResult> ShortenUrl([FromBody] UrlDto url)
        {
            if (url == null)
            {
                return BadRequest("Invalid input");
            }

            string shortUrl = await _urlShortener.ShortenUrlAsync(url);
            var response = new UrlShortResponseDto { Url = shortUrl };
            return Ok(response);
        }
    }
}
