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

namespace LinkShortener.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ShortenerController : ControllerBase
    {
        private readonly LinksService _linksService;

        public ShortenerController(LinksService linksService)
        {
            _linksService = linksService;
        }

        [HttpGet]
        public async Task<List<Link>> GetAll()
        {
            return await _linksService.GetAllAsync();
        }

        [HttpPost]
        public async Task<CreateShortLinkResponse> Create(CreateShortLinkRequest request)
        {
            var cookieData = HttpContext.Request.Cookies["Key"];
            return await _linksService.CreateAsync(request, cookieData);
        }

    }
}
