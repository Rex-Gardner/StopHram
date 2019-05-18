using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Models.Tags.Repositories
{
    public class MongoTagRepository : ITagRepository
    {
        private readonly IMongoCollection<Tag> placeTypes;

        public MongoTagRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("UrbanDb");
            placeTypes = database.GetCollection<Tag>("Tags");
        }
        
        public Task<Tag> CreateAsync(TagCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<Tag> GetAsync(string id, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<Tag>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<Tag> PatchAsync(TagPatchInfo patchInfo, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveAsync(string id, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}