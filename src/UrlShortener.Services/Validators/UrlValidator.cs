using FluentValidation;
using UrlShortener.Domain.ViewModels;

namespace UrlShortener.Services.Validators
{
    public class UrlValidator : AbstractValidator<UrlViewModel>
    {
        private const string URL_MATCH_REGEX = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";

        public UrlValidator()
        {
            RuleFor(url => url.OriginalUrl).NotEmpty().WithMessage("You must enter a URL")
                .Matches( URL_MATCH_REGEX ).WithMessage( "Provided URL is incorect" ); ;
            RuleFor(url => url.ShortUrl).NotEmpty().WithMessage("You must enter an alias");
        }
    }
}
