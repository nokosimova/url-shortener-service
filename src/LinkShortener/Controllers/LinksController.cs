using System;
using System.Threading.Tasks;
using LinkShortener.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.Controllers
{
    [ApiController]
    [Route("/")]
    public class LinksController : ControllerBase
    {
        private readonly LinksService _linksService;

        public LinksController(LinksService linksService)
        {
            _linksService = linksService;
        }

        [Route("{shortLink}")]
        [HttpGet]
        [ProducesResponseType(301)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Follow(string shortLink)
        {
            var originalUrl = await _linksService.GetOriginalLinkAsync(shortLink);
            if (originalUrl.OriginalLink == null)  
            {
                return NotFound();
            }
            return RedirectPermanent(originalUrl.OriginalLink);
        }
    }
}
