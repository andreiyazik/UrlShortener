using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UrlShortener.Domain.Entities;
using UrlShortener.Persistence.Contracts;

namespace UrlShortener.Persistence.Implementation.SQLServer
{
    public class UrlRepository : RepositoryBase<Url>, IUrlRepository
    {
        public UrlRepository(DataContext dataContext)
            : base(dataContext)
        {
        }

        public async Task DeleteAsync(string key)
        {
            var urlEntity = await GetAsync(key);
            _entitySet.Remove( urlEntity );

            await SaveChangesAsync();
        }

        public async Task<Url> FindAsync(string key)
        {
            return await _entitySet.SingleOrDefaultAsync(u => u.ShortUrl == key);
        }

        public async Task<Url> GetAsync(string key)
        {
            var urlEntity = await _entitySet.SingleOrDefaultAsync(u => u.ShortUrl == key);

            if(urlEntity == null)
            {
                throw new Exception($"Can't find Url with Key = '{key}'");
            }

            return urlEntity;
        }
    }
}