using System.Threading.Tasks;
using LinkShortener.Models.DTO.Requests;
using LinkShortener.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LinkShortener.Tests
{
    public class LinkService_GetAllAsync
    {
        private readonly LinksService _service;

        public LinkService_GetAllAsync(LinksService service)
        {
            _service = service;
        }

        [Fact]
        public async Task GetAllAsync_FromIncognitoMode_Result()
        {
            // Arrange

            // Act
            var links = await _service.GetAllLinksAsync(null);
                
            // Assert
            Assert.Empty(links);
        }
        
        [Fact]
        public async Task GetAllAsync_FroUsualMode_Result()
        {
            // Arrange

            // Act
            var links = await _service.GetAllLinksAsync(null);

            // Assert
            Assert.NotEmpty(links);
        }
    }
}