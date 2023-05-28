using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shortener.Data
{
    public interface IShortenersRepository
    {
        public Task<string> GetShortUrl(string longUrl);
        public Task<string> GetLongUrl(string shortUrl);
    }
}