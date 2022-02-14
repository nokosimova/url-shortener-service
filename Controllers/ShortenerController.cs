using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkShortener.Models;
using LinkShortener.Models.DTO.Responses;
using LinkShortener.Models.DTO.Requests;
using LinkShortener.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace LinkShortener.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShortenerController : ControllerBase
    {
        private readonly LinksService _linksService;

        public ShortenerController(LinksService linksService)
        {
            _linksService = linksService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateShortLinkRequest request)
        {
            var cookieData = HttpContext.Request.Cookies["Key"];
            var newLink = await _linksService.CreateAsync(request, cookieData);
            return Ok(newLink);
        }

        // [HttpGet]
        // public async Task<RedirectResult> Follow([FromQuery]FollowShortLinkRequest request)
        // {
        //     var originalUrl = await _linksService.GetOriginalLinkAsync(request.ShortLink);
        //     return RedirectPermanent(originalUrl);
        // }

        [HttpGet("getOrigin")]
        public async Task<IActionResult> GetOriginalLink([FromQuery]GetOriginalLinkRequest request)
        {
            var result = await _linksService.GetOriginalLinkAsync(request.ShortLink);
            return result == null ? NoContent() : Ok(result);
        }


        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllLinks()
        {
            var result =  await _linksService.GetAllLinksAsync();
            return result == null ? NoContent() : Ok(result);
        }
    }
}
