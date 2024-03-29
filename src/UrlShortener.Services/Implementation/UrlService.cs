﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Domain.Exceptions;
using UrlShortener.Domain.ViewModels;
using UrlShortener.Persistence.Contracts;
using UrlShortener.Services.Contracts;
using UrlShortener.Services.Validators;

namespace UrlShortener.Services.Implementation
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _urlRepository;
        private readonly ICacheService _cacheService;

        public UrlService(IUrlRepository urlRepository, ICacheService cacheService)
        {
            _urlRepository = urlRepository;
            _cacheService = cacheService;
        }

        public async Task CreateUrlAsync(UrlViewModel urlViewModel)
        {
            await ValidateUrl(urlViewModel);
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

        public async Task<UrlViewModel> GetUrlDetailsByKeyAsync(string key)
        {
            var originalUrl = await GetUrlByKeyAsync(key);

            return new UrlViewModel { OriginalUrl = originalUrl, ShortUrl = key };
        }

        private async Task ValidateUrl(UrlViewModel urlViewModel)
        {
            var errors = new List<string>();

            var validator = new UrlValidator();
            var result = validator.Validate(urlViewModel);
            errors.AddRange(result.Errors?.Select(e => e.ErrorMessage));

            if (_cacheService.FindItem<string>(urlViewModel.ShortUrl) != null ||
                await _urlRepository.FindAsync( urlViewModel.ShortUrl) != null)
            {
                errors.Add("Alias already exists");
            }

            if (errors.Count > 0)
            {
                throw new UrlValidationException("One or more errors occured", errors);
            }
        }

        private void StandardiseUrl( UrlViewModel urlViewModel )
        {
            if(!urlViewModel.OriginalUrl.StartsWith("http"))
            {
                urlViewModel.OriginalUrl = $"http://{urlViewModel.OriginalUrl}";
            }
        }
    }
}