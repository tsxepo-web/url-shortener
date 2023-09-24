using SharedModels;

namespace DataAccessLayer.Repositories
{
    public interface IUrlMappingRepository
    {
        Task<UrlMapping> FindByLongUrlAsync(UrlDto url);
        Task<UrlMapping> FindByShortUrlAsync();
        Task InsertAsync(UrlMapping urlMapping);
    }
}
