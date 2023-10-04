using SharedModels;

namespace DataAccessLayer.Repositories
{
    public interface IUrlMappingRepository
    {
        Task<UrlMapping> FindByLongUrlAsync(UrlDto url);
        Task<UrlMapping> FindByShortUrlAsync(string path);
        Task InsertAsync(UrlMapping urlMapping);
    }
}
