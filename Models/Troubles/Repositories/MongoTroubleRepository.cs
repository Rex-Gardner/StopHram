using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models.Troubles.Exceptions;
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
                Images = new string[0],
                Coordinates = creationInfo.Coordinates,
                Address = creationInfo.Address,
                Tags = creationInfo.Tags,
                Status = TroubleStatus.Created,
                CreatedAt = now,
                LastUpdateAt = now
            };

            troubles.InsertOne(trouble, cancellationToken: cancellationToken);
            return Task.FromResult(trouble);
        }

        public Task<IReadOnlyList<Trouble>> SearchAsync(TroubleSearchInfo searchInfo, CancellationToken cancellationToken)
        {
            if (searchInfo == null)
            {
                throw new ArgumentNullException(nameof(searchInfo));
            }
            
            cancellationToken.ThrowIfCancellationRequested();
            
            var search = troubles.Find(item => true).ToEnumerable();

            if (searchInfo.Tags != null)
            {
                search = search.Where(item => item.Tags.Any(tag => searchInfo.Tags.Contains(tag)));
            }

            if (searchInfo.Statuses != null)
            {
                search = search.Where(item => searchInfo.Statuses.Contains(item.Status));
            }

            if (searchInfo.Offset != null)
            {
                search = search.Skip(searchInfo.Offset.Value);
            }

            if (searchInfo.Limit != null)
            {
                search = search.Take(searchInfo.Limit.Value);
            }

            var result = search.ToList();
            return Task.FromResult<IReadOnlyList<Trouble>>(result);
        }

        public Task<Trouble> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var trouble = troubles.Find(item => item.Id == id).FirstOrDefault();
            
            if (trouble == null)
            {
                throw new TroubleNotFoundException(id.ToString());
            }

            return Task.FromResult(trouble);
        }

        public Task<Trouble> PatchAsync(TroublePatchInfo patchInfo, CancellationToken cancellationToken)
        {
            if (patchInfo == null)
            {
                throw new ArgumentNullException(nameof(patchInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();
            var trouble = troubles.Find(item => item.Id == patchInfo.Id).FirstOrDefault();

            if (trouble == null)
            {
                throw new TroubleNotFoundException(patchInfo.Id.ToString());
            }

            var updated = false;

            if (patchInfo.Name != null)
            {
                trouble.Name = patchInfo.Name;
                updated = true;
            }
            
            if (patchInfo.Description != null)
            {
                trouble.Description = patchInfo.Description;
                updated = true;
            }
            
            if (patchInfo.Latitude != null && patchInfo.Longitude != null)
            {
                trouble.Coordinates = new GeoCoordinate(patchInfo.Latitude.Value, patchInfo.Longitude.Value);
                updated = true;
            }

            if (patchInfo.Address != null)
            {
                trouble.Address = patchInfo.Address;
                updated = true;
            }

            if (patchInfo.Tags != null && patchInfo.Tags.Any())
            {
                trouble.Tags = patchInfo.Tags;
                updated = true;
            }

            if (patchInfo.Status != null)
            {
                trouble.Status = patchInfo.Status.Value;
                updated = true;
            }

            if (updated)
            {
                trouble.LastUpdateAt = DateTime.UtcNow;
                troubles.ReplaceOne(item => item.Id == patchInfo.Id, trouble);
            }

            return Task.FromResult(trouble);
        }

        public Task RemoveAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}