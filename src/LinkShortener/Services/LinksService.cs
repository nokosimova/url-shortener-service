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
        
        private const int _shortLength = 5;
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private static Random _random = new Random();
        private static string _baseUrl;


        public LinksService(IOptions<DatabaseSettingModel> DatabaseSetting, IHttpContextAccessor accessor)
        {
            // connect to database:
            var mongoClient = new MongoClient(DatabaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(DatabaseSetting.Value.DatabaseName);            
            _links = mongoDatabase.GetCollection<Link>(DatabaseSetting.Value.CollectionName);

             //get base Url for generating short links:
            _baseUrl = $"{accessor.HttpContext.Request.Scheme}://{accessor.HttpContext.Request.Host.ToUriComponent()}/";
        }

        public async Task<CreateShortLinkResponse> CreateAsync(CreateShortLinkRequest request, string cookieData = null)
        {
            if (!Uri.IsWellFormedUriString(request.OriginalLink, UriKind.Absolute))
                throw new BadHttpRequestException("Incorrect url format");
            
            var filter = Builders<Link>.Filter.Eq(x => x.LinkName, request.OriginalLink);
            var link = await _links.Find(filter).FirstOrDefaultAsync();

            if (link == null)
            {
                var newShortLink = await CreateShortLink(); 
                link = new Link{
                    LinkName = request.OriginalLink,
                    CookieValue = cookieData,
                    ShortName = _baseUrl + newShortLink
                };
                await _links.InsertOneAsync(link);           
            }

            return new CreateShortLinkResponse{
                    OriginalLink = link.LinkName,
                    ShortLink = _baseUrl + link.ShortName
                };
        }

        public async Task<List<GetLinkItemResponse>> GetAllLinksAsync(string cookieData = null)
        {
            var links = await _links.Find(Builders<Link>.Filter.Empty).ToListAsync();

            return links.Select(x => new GetLinkItemResponse{
                        OriginalLink = x.LinkName,
                        ShortLink = _baseUrl + x.ShortName,
                        VisitsCount = x.VisitsCount
                    }).ToList();
        }


        public async Task<GetOriginalLinkResponse> GetOriginalLinkAsync(string shortLink)
        {
            var filter = Builders<Link>.Filter.Eq(x => x.ShortName, shortLink);
            var link = await _links.Find(filter).FirstOrDefaultAsync();
            
            if (link != null)
            {
                var update = Builders<Link>.Update.Set(x  => x.VisitsCount, link.VisitsCount + 1);
                await _links.UpdateOneAsync(filter, update);       
            }    
                        
            return new GetOriginalLinkResponse{
                OriginalLink = link?.LinkName
            };  
        }

        private async Task<string> CreateShortLink()
        {
            var result = await GenerateLink();
            var link = await _links.Find(Builders<Link>.Filter.Eq(x => x.ShortName, result)).ToListAsync();

            while (link.Any())
            {
                result = await GenerateLink();
                link = await _links.Find(Builders<Link>.Filter.Eq(x => x.ShortName, result)).ToListAsync();
            }

            return new string(result);
        }

        private async Task<string> GenerateLink()
        {
            var result = new string(Enumerable.Range(1, _shortLength).
                                Select(_ => _chars[_random.Next(_chars.Length)]).ToArray());
            return result;
        }
    }
}