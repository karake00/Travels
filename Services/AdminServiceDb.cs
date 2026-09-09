using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using DbRepos;
using Models.DTO;

namespace Services;

public class AdminServiceDb : IAdminService
{
    private readonly AdminDbRepos _repo = null;
    private readonly ILogger<AdminServiceDb> _logger = null;

    #region constructors
    public AdminServiceDb(AdminDbRepos repo)
    {
        _repo = repo;
    }

    public AdminServiceDb(AdminDbRepos repo, ILogger<AdminServiceDb> logger) : this(repo)
    {
        _logger = logger;
    }
    #endregion

    // Tjänsten skickar bara anropen vidare 1:1 till ditt AdminDbRepos
    public Task<ResponseItemDto<GstUsrInfoAllDto>> GuestInfoAsync() => _repo.InfoAsync();
    public Task<ResponseItemDto<GstUsrInfoAllDto>> SeedAsync(int nrOfItems) => _repo.SeedAsync(nrOfItems);
    public Task<ResponseItemDto<GstUsrInfoAllDto>> RemoveSeedAsync(bool seeded) => _repo.RemoveSeedAsync(seeded);
}
