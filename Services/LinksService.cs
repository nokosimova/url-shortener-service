using Microsoft.Extensions.Options;
using MongoDB.Driver;
using LinkShortener.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LinkShortener.Services{
    public class LinksService
    {
        private readonly IMongoCollection<Link> _links;

        public LinksService(IOptions<DatabaseSettingModel> DatabaseSetting)
        {
            var mongoClient = new MongoClient(DatabaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(DatabaseSetting.Value.DatabaseName);
            
            _links = mongoDatabase.GetCollection<Link>(DatabaseSetting.Value.CollectionName);
        }

        public async Task<List<Link>> GetAsync() =>
            await _links.Find(_ => true).ToListAsync();

    }
}