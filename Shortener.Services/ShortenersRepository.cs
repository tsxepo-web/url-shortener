using MongoDB.Driver;
using Shortener.Data;
using Shortener.Models;

namespace Shortener.Service;
public class ShortenersRepository : IShortenersRepository
{
    private readonly IMongoCollection<UrlMapping> _mongoCollection;
    public ShortenersRepository(IMongoCollection<UrlMapping> mongoCollection)
    {
        _mongoCollection = mongoCollection;
    }
    public async Task<string> GetShortUrl(string longUrl)
    {
        if (ValidateUrl(longUrl))
        {
            var existingMapping = await _mongoCollection.Find(x => x.LongUrl == longUrl).FirstOrDefaultAsync();
            if (existingMapping == null)
            {
                var shortUrl = CreateShortUrl(longUrl);
                return await shortUrl;
            }
            return existingMapping.ShortUrl;
        }
        return "The url scheme is invalid.";
    }
    public bool ValidateUrl(string longUrl)
    {
        return Uri.TryCreate(longUrl, UriKind.Absolute, out Uri? uriResult) &&
        (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
    public string GenerateShortUrl()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        string shortUrl = new(Enumerable.Repeat(chars, 7).Select(s => s[random.Next(s.Length)]).ToArray());
        return shortUrl;
    }
    public async Task<string> CreateShortUrl(string longUrl)
    {
        var shortUrl = "tx.nano/" + GenerateShortUrl();
        var newMapping = new UrlMapping
        {
            LongUrl = longUrl,
            ShortUrl = shortUrl,
            AccessTimestamps = new List<DateTime> { DateTime.Now }
        };
        await _mongoCollection.InsertOneAsync(newMapping);
        return shortUrl;
    }

    public async Task<string> GetLongUrl(string shortUrl)
    {
        try
        {
            var decodedUrl = Uri.UnescapeDataString(shortUrl);
            var mapping = await _mongoCollection.Find(x => x.ShortUrl == decodedUrl).FirstOrDefaultAsync();
            var longUrl = mapping.LongUrl;
            mapping.AccessTimestamps?.Add(DateTime.UtcNow);
            await _mongoCollection.ReplaceOneAsync(x => x.LongUrl == longUrl, mapping);
            return longUrl;
        }
        catch (Exception)
        {
            throw new Exception("ShortUrl not found. Try creating a new one.");
        }
    }
}