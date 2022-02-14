using Microsoft.Extensions.Options;
using MongoDB.Driver;
using LinkShortener.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using LinkShortener.Models.DTO.Responses;
using LinkShortener.Models.DTO.Requests;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;
using System.Text;

namespace LinkShortener.Services{
    public class LinksService
    {
        private readonly IMongoCollection<Link> _links;
        
        private const int shortLinkLength = 5;
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        
        private static Random random = new Random();
        private static string baseUrl;


        public LinksService(IOptions<DatabaseSettingModel> DatabaseSetting, IHttpContextAccessor accessor)
        {
            // connect to database:
            var mongoClient = new MongoClient(DatabaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(DatabaseSetting.Value.DatabaseName);            
            _links = mongoDatabase.GetCollection<Link>(DatabaseSetting.Value.CollectionName);

            //get base Url for generating short links:
            baseUrl = $"{accessor.HttpContext.Request.Scheme}://{accessor.HttpContext.Request.Host.ToUriComponent()}";

        }

        public async Task<List<Link>> GetAllAsync() =>
            await _links.Find(_ => true).ToListAsync();

        public async Task<CreateShortLinkResponse> CreateAsync(CreateShortLinkRequest request, string cookieData)
        {
            var link = await _links.Find(x => x.CookieValue == (cookieData??x.CookieValue)
                                && x.LinkName == request.OriginalLink).FirstOrDefaultAsync();
            if (link != null)
            {
                return new CreateShortLinkResponse {
                    OriginalLink = link.LinkName,
                    ShortLink = link.ShortName
                };
            }

            var newLink = new Link{
                LinkName = request.OriginalLink,
                CookieValue = cookieData,
                ShortName = GenerateShortLink()
            };
            await _links.InsertOneAsync(newLink);
            return new CreateShortLinkResponse{
                    OriginalLink = newLink.LinkName,
                    ShortLink = newLink.ShortName
                };
        }

        private string GenerateShortLink()
        {
            var result = new string(Enumerable.Range(1, shortLinkLength).
                                Select(_ => chars[random.Next(chars.Length)]).ToArray());
            
            while (_links.Find(x => x.ShortName == result).FirstOrDefault() != null)
                result = new string(Enumerable.Range(1, shortLinkLength).
                         Select(_ => chars[random.Next(chars.Length)]).ToArray());
            return baseUrl + '/' + result;
        }

        private void IncreaseVisitCount(Link link) => link.VisitsCount ++;
    }
}