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
    [Route("api/links")]
    public class ShortenerController : ControllerBase
    {
        private readonly LinksService _linksService;

        public ShortenerController(LinksService linksService)
        {
            _linksService = linksService;
        }

        [HttpPost("new")]
        public async Task<CreateShortLinkResponse> Create(CreateShortLinkRequest request)
        {
            var cookieData = HttpContext.Request.Cookies["Key"];
            var newLink = await _linksService.CreateAsync(request, cookieData);
            return newLink;
        }

        // [HttpGet]
        // public async Task<RedirectResult> Follow([FromRoute] string ShortLink)
        // {
        //     var originalUrl = await _linksService.GetOriginalLinkAsync(ShortLink);
        //     return RedirectPermanent(originalUrl.OriginalLink);
        // }

        [HttpGet("origin")]
        public async Task<GetOriginalLinkResponse> GetOriginalLink([FromQuery]GetOriginalLinkRequest request)
        {
            var result = await _linksService.GetOriginalLinkAsync(request.ShortLink);
            if (result == null) 
            {
                Response.StatusCode = 404;
            }    
            return result;
        }


        [HttpGet("all")]
        public async Task<List<GetLinkItemResponse>> GetAllLinks()
        {
            var result =  await _linksService.GetAllLinksAsync();
            if (result == null) 
            {
                Response.StatusCode = 204;
            }    
            return result;
        }
    }
}
