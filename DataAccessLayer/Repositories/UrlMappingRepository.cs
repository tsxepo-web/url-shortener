using MongoDB.Driver;
using SharedModels;

namespace DataAccessLayer.Repositories
{
    public class UrlMappingRepository : IUrlMappingRepository
    {
        private readonly IMongoCollection<UrlMapping> _mongoCollection;
        public UrlMappingRepository(IMongoCollection<UrlMapping> mongoCollection)
        {
            _mongoCollection = mongoCollection;
        }
        public async Task<UrlMapping> FindByShortUrlAsync(string path)
        {
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
