using System.Threading.Tasks;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Persistence.Contracts
{
    public interface IUrlRepository : IRepositoryBase<Url>
    {
        Task<Url> GetAsync(string key);
        Task<Url> FindAsync(string key);
        Task DeleteAsync(string key);
    }
}