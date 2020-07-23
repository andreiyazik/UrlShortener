using System.Threading.Tasks;
using UrlShortener.Domain.ViewModels;

namespace UrlShortener.Services.Contracts
{
    public interface IUrlService
    {
        Task CreateUrlAsync(UrlViewModel urlViewModel);
        Task<string> GetUrlByKeyAsync(string key);
        Task DeleteUrlAsync(string key);
    }
}