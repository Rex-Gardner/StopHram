using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Models.Troubles.Repositories
{
    public class MongoTroubleRepository : ITroubleRepository
    {
        private readonly IMongoCollection<Trouble> troubles;

        public MongoTroubleRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("UrbanDb");
            troubles = database.GetCollection<Trouble>("Troubles");
        }
        
        public Task<Trouble> CreateAsync(TroubleCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();
            //todo check TroubleDuplication

            var guid = Guid.NewGuid();
            var now = DateTime.UtcNow;    //todo Local time
            
            var trouble = new Trouble
            {
                Id = guid,
                Name = creationInfo.Name,
                Description = creationInfo.Description,
                Images = creationInfo.Images,
                Coordinates = creationInfo.Coordinates,
                Address = creationInfo.Address,
                Tags = creationInfo.Tags,
                Status = creationInfo.Status,
                CreatedAt = now,
                LastUpdateAt = now
            };

            troubles.InsertOne(trouble, cancellationToken: cancellationToken);
            return Task.FromResult(trouble);
        }

        public Task<IReadOnlyList<Trouble>> SearchAsync(TroubleSearchInfo searchInfo, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<Trouble> GetAsync(string id, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<Trouble> PatchAsync(TroublePatchInfo patchInfo, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveAsync(string id, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}