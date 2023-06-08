using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shortener.Models;

namespace Shortener.Data
{
    public interface IShortenersRepository
    {
        public Task<string> GetShortUrl(string longUrl);
        public Task<string> GetLongUrl(string shortUrl);
        public Task<IEnumerable<UrlMapping>> GetAllMappings();
    }
}