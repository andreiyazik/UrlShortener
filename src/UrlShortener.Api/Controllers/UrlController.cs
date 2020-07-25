using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UrlShortener.Domain.Exceptions;
using UrlShortener.Domain.ViewModels;
using UrlShortener.Services.Contracts;

namespace UrlShortener.Api.Controllers
{
    [AllowAnonymous]
    public class UrlController :  ControllerBase
    {
        private readonly IUrlService _urlService;

        public UrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [Route("{key}")]
        [HttpGet]
        public async Task<IActionResult> GetUrl(string key)
        {
            try
            {
                var url = await _urlService.GetUrlByKeyAsync(key);
                return Redirect(url);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("details/{key}")]
        [HttpGet]
        public async Task<IActionResult> GetUrlDetails(string key)
        {
            try
            {
                var details = await _urlService.GetUrlDetailsByKeyAsync(key);
                return Ok(details);
            }
            catch (Exception ex)
            {
                return BadRequest( ex.Message );
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateUrl([FromBody]UrlViewModel urlViewModel)
        {
            try
            {
                await _urlService.CreateUrlAsync(urlViewModel);
                return Created("", urlViewModel);
            }
            catch(UrlValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUrl(string key)
        {
            try
            {
                await _urlService.DeleteUrlAsync(key);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}