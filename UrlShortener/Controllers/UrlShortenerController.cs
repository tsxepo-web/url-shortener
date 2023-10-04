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
        private readonly IHttpContextAccessor _contextAccessor;
        public UrlShortenerController(IUrlShortener urlShortener, IHttpContextAccessor contextAccessor)
        {
            _urlShortener = urlShortener;
            _contextAccessor = contextAccessor;
        }

        [HttpPost("shortUrl")]
        public async Task<IActionResult> ShortenUrl([FromBody] UrlDto url)
        {
            if (url == null)
            {
                return BadRequest("Invalid input");
            }
            string shortUrl = await _urlShortener.ShortenUrlAsync(url);
            string baseUrl = $"{_contextAccessor.HttpContext!.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}";
            string res = $"{baseUrl}/{shortUrl}";
            var response = new UrlShortResponseDto { Url = res };
            return Ok(response);
        }
    }
}
