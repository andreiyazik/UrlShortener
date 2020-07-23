using Microsoft.Extensions.Caching.Memory;
using System;
using UrlShortener.Services.Contracts;

namespace UrlShortener.Services.Implementation
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void PutItem<T>(string key, T item)
        {
            _cache.Set(key, item,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
        }

        public T FindItem<T>(string key)
        {
            _cache.TryGetValue<T>(key, out T item);
            return item;
        }
    }
}