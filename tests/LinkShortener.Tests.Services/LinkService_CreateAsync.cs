using System.Threading.Tasks;
using LinkShortener.Controllers;
using LinkShortener.Models.DTO.Requests;
using LinkShortener.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LinkShortener.Tests
{
    public class LinkService_CreateAsync
    {
        private readonly LinksService _service;

        public LinkService_CreateAsync(LinksService service)
        {
            _service = service;
        }

        [Fact]
        public async Task CreateAsync_Success_Result()
        {
            // Arrange
            var correctLinkItem = new CreateShortLinkRequest()
            {
                OriginalLink = "https://www.youtube.com/"
            };
            
            // Act
            var response = await _service.CreateAsync(correctLinkItem);
                
            // Assert
            Assert.Equal(correctLinkItem.OriginalLink, response.OriginalLink);
        }
        
        [Fact]
        public async Task CreateAsync_IncorrectInput_BadRequest_Result()
        {
            // Arrange
            var incorrectLinkItem = new CreateShortLinkRequest()
            {
                OriginalLink = "hhttp:/fd;igjfsdh.com"
            };
            
            // Act
            var badResponse = await _service.CreateAsync(incorrectLinkItem);
                
            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }
        
    }
}