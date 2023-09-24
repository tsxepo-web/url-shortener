using SharedModels;

namespace BusinessLogicLayer.Services
{
    public interface IUrlShortener
    {
        Task<string> ShortenUrlAsync(UrlDto url);
    }
}
