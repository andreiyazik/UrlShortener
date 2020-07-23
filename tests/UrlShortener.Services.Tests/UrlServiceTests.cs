using NUnit.Framework;
using UrlShortener.Domain.ViewModels;
using UrlShortener.Services.Contracts;
using UrlShortener.Services.Tests.Services;

namespace UrlShortener.Services.Tests
{
    public class UrlServicesTest
    {
        private IUrlService _urlService;

        [SetUp]
        public void Setup()
        {
            _urlService = new FakeUrlService();
        }

        [Test]
        public void Url_Should_Be_Created_Received_And_Deleted_Successfully()
        {
            var url = new UrlViewModel { OriginalUrl = "www.google.com", ShortUrl = "googl"  };
            Assert.That(() => _urlService.CreateUrlAsync(url), Throws.Nothing);
            var result = _urlService.GetUrlByKeyAsync("googl");
            Assert.IsNotNull(result);

            Assert.That(() => _urlService.DeleteUrlAsync("googl"), Throws.Nothing);
        }

        [Test]
        public void Url_With_Empty_Original_Url_Will_Not_Be_Created()
        {
            var url = new UrlViewModel { ShortUrl = "googl" };
            Assert.That(() => _urlService.CreateUrlAsync(url), Throws.Exception);
        }

        [Test]
        public void Url_With_Empty_Alias_Will_Not_Be_Created()
        {
            var url = new UrlViewModel { OriginalUrl = "www.google.com" };
            Assert.That(() => _urlService.CreateUrlAsync( url ), Throws.Exception);
        }

        [Test]
        public void Url_With_Incorrect_Original_Url_Will_Not_Be_Created()
        {
            var url = new UrlViewModel { OriginalUrl = "googl", ShortUrl = "googl" };
            Assert.That(() => _urlService.CreateUrlAsync(url), Throws.Exception);
        }

        [Test]
        public void Url_With_Duplicate_Alias_Will_Not_Be_Created()
        {
            var url = new UrlViewModel { OriginalUrl = "www.google.com", ShortUrl = "googl" };
            Assert.That(() => _urlService.CreateUrlAsync( url ), Throws.Nothing);

            var anotherUrl = new UrlViewModel { OriginalUrl = "www.google.eu", ShortUrl = "googl" };
            Assert.That(() => _urlService.CreateUrlAsync( url ), Throws.Exception);

            Assert.That(() => _urlService.DeleteUrlAsync("googl"), Throws.Nothing);
        }
    }
}