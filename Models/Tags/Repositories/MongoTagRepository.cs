using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.Tags.Exceptions;
using MongoDB.Driver;

namespace Models.Tags.Repositories
{
    public class MongoTagRepository : ITagRepository
    {
        private readonly IMongoCollection<Tag> tags;

        public MongoTagRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("UrbanDb");
            tags = database.GetCollection<Tag>("Tags");
        }
        
        public Task<Tag> CreateAsync(TagCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();
            var tagWithSameId = tags.Find(item => item.Id == creationInfo.Id).FirstOrDefault();

            if (tagWithSameId != null)
            {
                throw new TagDuplicationException(creationInfo.Id);
            }

            var tag = new Tag
            {
                Id = creationInfo.Id,
                Name = creationInfo.Name
            };

            tags.InsertOne(tag, cancellationToken: cancellationToken);
            return Task.FromResult(tag);
        }

        public Task<Tag> GetAsync(string id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            
            cancellationToken.ThrowIfCancellationRequested();
            var tag = tags.Find(item => item.Id == id).FirstOrDefault();
            
            if (tag == null)
            {
                throw new TagNotFoundException(id);
            }

            return Task.FromResult(tag);
        }

        public Task<IReadOnlyList<Tag>> GetAllAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var allTags = tags.Find(item => true).ToList();
            return Task.FromResult<IReadOnlyList<Tag>>(allTags);
        }

        public Task<Tag> PatchAsync(TagPatchInfo patchInfo, CancellationToken cancellationToken)
        {
            if (patchInfo == null)
            {
                throw new ArgumentNullException(nameof(patchInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();
            var tag = tags.Find(item => item.Id == patchInfo.Id).FirstOrDefault();

            if (tag == null)
            {
                throw new TagNotFoundException(patchInfo.Id);
            }

            if (string.IsNullOrEmpty(patchInfo.Name))
            {
                return Task.FromResult(tag);
            }
            
            tag.Name = patchInfo.Name;
            tags.ReplaceOne(item => item.Id == patchInfo.Id, tag);
            return Task.FromResult(tag);
        }

        public Task RemoveAsync(string id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            cancellationToken.ThrowIfCancellationRequested();
            var deleteResult = tags.DeleteOne(type => type.Id == id);

            if (deleteResult.DeletedCount == 0)
            {
                throw new TagNotFoundException(id);
            }

            return Task.CompletedTask;
        }
    }
}