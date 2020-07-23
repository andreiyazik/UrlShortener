using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.ViewModels;
using UrlShortener.Persistence.Contracts;
using UrlShortener.Services.Contracts;
using UrlShortener.Services.Exceptions;

namespace UrlShortener.Services.Implementation
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _urlRepository;
        private readonly ICacheService _cacheService;

        private const string URL_MATCH_REGEX = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";

        public UrlService(IUrlRepository urlRepository, ICacheService cacheService)
        {
            _urlRepository = urlRepository;
            _cacheService = cacheService;
        }

        public async Task CreateUrlAsync(UrlViewModel urlViewModel)
        {
            ValidateUrl(urlViewModel);
            var urlEntity = urlViewModel.CreateEntity();
            await _urlRepository.CreateAsync(urlEntity);
            _cacheService.PutItem(urlEntity.ShortUrl, urlEntity.OriginalUrl);
        }

        public async Task DeleteUrlAsync(string key)
        {
            await _urlRepository.DeleteAsync(key);
        }

        public async Task<string> GetUrlByKeyAsync(string key)
        {
            var url = _cacheService.FindItem<string>(key);
            if(null != url)
            {
                return url;
            }

            var urlEntity = await _urlRepository.GetAsync(key);
            _cacheService.PutItem(urlEntity.ShortUrl, urlEntity.OriginalUrl);

            return urlEntity.OriginalUrl;
        }

        private void ValidateUrl(UrlViewModel urlViewModel)
        {
            var errors = new List<string>();

            if(string.IsNullOrWhiteSpace(urlViewModel.OriginalUrl))
            {
                errors.Add("You must enter a URL");
            }
            if (string.IsNullOrWhiteSpace(urlViewModel.ShortUrl))
            {
                errors.Add("You must enter an alias");
            }
            if(!Regex.IsMatch(urlViewModel.OriginalUrl, URL_MATCH_REGEX))
            {
                errors.Add( "Provided URL is incorect");
            }

            if (_cacheService.FindItem<string>(urlViewModel.ShortUrl) != null ||
                _urlRepository.FindAsync(urlViewModel.ShortUrl) != null)
            {
                errors.Add("Alias already exists");
            }

            if (errors.Count > 0)
            {
                throw new UrlValidationException("One or more errors occured", errors);
            }
        }
    }
}
