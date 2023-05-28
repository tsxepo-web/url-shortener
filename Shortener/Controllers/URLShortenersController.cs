using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shortener.Data;

namespace Shortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class URLShortenersController : ControllerBase
    {
        private readonly IShortenersRepository _shortenersRepository;
        public URLShortenersController(IShortenersRepository shortenersRepository)
        {
            _shortenersRepository = shortenersRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateShortUrl(string longUrl)
        {
            string shortUrl = await _shortenersRepository.GetShortUrl(longUrl);
            return Ok(shortUrl);
        }
        [HttpGet("{shortUrl}")]
        public new IActionResult Redirect(string shortUrl)
        {
            string longUrl = _shortenersRepository.GetLongUrl(shortUrl).Result;
            return new RedirectResult(longUrl);
        }
    }
}