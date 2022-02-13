using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkShortener.Models;
using LinkShortener.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        [HttpGet]
        public async Task<List<Link>> Get()
        {
            return await _linksService.GetAsync();
        }

    }
}
