using BusinessLogicLayer.Services;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlShortenerController : ControllerBase
    {
        private readonly IUrlShortener _urlShortener;
        private readonly IUrlMappingRepository _repository;

        public UrlShortenerController(IUrlShortener urlShortener, IUrlMappingRepository repository)
        {
            _urlShortener = urlShortener;
            _repository = repository;
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
