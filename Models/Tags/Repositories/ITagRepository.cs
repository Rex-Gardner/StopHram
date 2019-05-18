using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Tags.Repositories
{
    public interface ITagRepository
    {
        Task<Tag> CreateAsync(TagCreationInfo creationInfo, CancellationToken cancellationToken);
        Task<Tag> GetAsync(string id, CancellationToken cancellationToken);
        Task<IReadOnlyList<Tag>> GetAllAsync(CancellationToken cancellationToken);
        Task<Tag> PatchAsync(TagPatchInfo patchInfo, CancellationToken cancellationToken);
        Task RemoveAsync(string id, CancellationToken cancellationToken);
    }
}