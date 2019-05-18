using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Troubles.Repositories
{
    public interface ITroubleRepository
    {
        Task<Trouble> CreateAsync(TroubleCreationInfo creationInfo, CancellationToken cancellationToken);
        Task<IReadOnlyList<Trouble>> SearchAsync(TroubleSearchInfo searchInfo, CancellationToken cancellationToken);
        Task<Trouble> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<Trouble> PatchAsync(TroublePatchInfo patchInfo, CancellationToken cancellationToken);
        Task RemoveAsync(Guid id, CancellationToken cancellationToken);
    }
}