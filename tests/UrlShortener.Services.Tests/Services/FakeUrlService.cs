using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Exceptions;
using UrlShortener.Domain.ViewModels;
using UrlShortener.Services.Contracts;
using UrlShortener.Services.Validators;

namespace UrlShortener.Services.Tests.Services
{
    public class FakeUrlService : IUrlService
    {
        private readonly List<Url> urls;

        public FakeUrlService()
        {
            urls = new List<Url>();
        }

        public async Task CreateUrlAsync(UrlViewModel urlViewModel)
        {
            ValidateUrl(urlViewModel);
            var urlEntity = urlViewModel.CreateEntity();
            urls.Add(urlEntity);
        }

        public async Task DeleteUrlAsync(string key)
        {
            var urlEntity = GetUrlByKey( key );
            urls.Remove(urlEntity);
        }

        public Task<string> GetUrlByKeyAsync(string key)
        {
            var urlEntity = GetUrlByKey(key);
            return Task.FromResult(urlEntity.OriginalUrl);
        }

        public Task<UrlViewModel> GetUrlDetailsByKeyAsync(string key)
        {
            throw new NotImplementedException();
        }

        private Url GetUrlByKey(string key)
        {
            var urlEntity = urls.FirstOrDefault(u => u.ShortUrl == key);

            if (urlEntity == null)
            {
                throw new Exception($"Can't find Url with Key = '{key}'");
            }

            return urlEntity;
        }

        private void ValidateUrl(UrlViewModel urlViewModel)
        {
            var errors = new List<string>();

            var validator = new UrlValidator();
            var result = validator.Validate(urlViewModel);
            errors.AddRange( result.Errors.Select(e => e.ErrorMessage));

            if (urls.FirstOrDefault(u => u.ShortUrl == urlViewModel.ShortUrl) != null)
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