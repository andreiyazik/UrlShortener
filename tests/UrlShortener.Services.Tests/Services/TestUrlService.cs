using System.Collections.Generic;
using System.Threading.Tasks;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.ViewModels;
using UrlShortener.Services.Contracts;

namespace UrlShortener.Services.Tests.Services
{
    public class TestUrlService : IUrlService
    {
        private List<Url> urls;

        public TestUrlService()
        {
            urls = new List<Url>();
        }

        public Task CreateUrlAsync(UrlViewModel urlViewModel)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteUrlAsync(string key)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetUrlByKeyAsync(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}