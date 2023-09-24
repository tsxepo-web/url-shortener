using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using SharedModels;

namespace DataAccessLayer.Repositories
{
    public class UrlMappingRepository : IUrlMappingRepository
    {
        private readonly IMongoCollection<UrlMapping> _mongoCollection;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UrlMappingRepository(IMongoCollection<UrlMapping> mongoCollection, IHttpContextAccessor httpContextAccessor)
        {
            _mongoCollection = mongoCollection;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<UrlMapping> FindByShortUrlAsync()
        {
            var path = _httpContextAccessor.HttpContext!.Request.Path.ToUriComponent().Trim('/').ToLower();
            var urlMatch = await _mongoCollection.Find(x => x.ShortUrl.ToLower().Trim() == path).FirstOrDefaultAsync();
            return urlMatch ?? throw new Exception("Short URL not found.");
        }
        public async Task<UrlMapping> FindByLongUrlAsync(UrlDto url)
        {
            return await _mongoCollection.Find(x => x.Url == url.Url).FirstOrDefaultAsync();
        }
        public async Task InsertAsync(UrlMapping urlMapping)
        {
            await _mongoCollection.InsertOneAsync(urlMapping);
        }
    }
}
