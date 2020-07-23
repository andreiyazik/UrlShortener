using NUnit.Framework;
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
            _urlService = new TestUrlService();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}