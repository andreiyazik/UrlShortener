using System;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Domain.ViewModels
{
    public class UrlViewModel
    {
        public UrlViewModel()
        {

        }

        public UrlViewModel(Url url)
        {
            ShortUrl = url.ShortUrl;
            OriginalUrl = url.OriginalUrl;
        }

        public string ShortUrl { get; set; }
        public string OriginalUrl { get; set; }

        public Url CreateEntity()
        {
            return new Url
            {
                ShortUrl = ShortUrl,
                OriginalUrl = OriginalUrl,
                CreationDate = DateTime.UtcNow
            };
        }
    }
}