using DataAccessLayer.Repositories;
using SharedModels;

namespace BusinessLogicLayer.Services
{
    public class UrlShortenerService : IUrlShortener
    {
        private readonly IUrlMappingRepository _repository;
        public UrlShortenerService(IUrlMappingRepository repository)
        {
            _repository = repository;
        }
        public async Task<string> ShortenUrlAsync(UrlDto url)
        {
            string shortUrl;

            ValidateUrl(url);
            var existingMapping = await _repository.FindByLongUrlAsync(url);
            if (existingMapping != null)
            {
                shortUrl = existingMapping.ShortUrl;
            }
            else
            {
                shortUrl = GenerateShortUrl();
                var newMap = new UrlMapping
                {
                    Url = url.Url,
                    ShortUrl = shortUrl,
                };
                await _repository.InsertAsync(newMap);

            }
            return shortUrl;
        }
        private void ValidateUrl(UrlDto url)
        {
            if (!Uri.TryCreate(url.Url, UriKind.Absolute, result: out _))
            {
                throw new ArgumentException("Error validating URL, ensure that your URL has a valid URL scheme, http and https is allowed");
            }
        }
        private string GenerateShortUrl()
        {
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var randomChars = new char[8];

            for (int i = 0; i < randomChars.Length; i++)
            {
                randomChars[i] = allowedChars[random.Next(allowedChars.Length)];
            }

            return new string(randomChars);
        }
    }
}
