using Microsoft.Extensions.Logging;

using Models;
using Models.DTO;
using DbRepos;

namespace Services;

public class AttractionsServiceDb : IAttractionsService
{
    private readonly AttractionsDbRepos _repo = null;
    private readonly ILogger<AttractionsServiceDb> _logger = null;

    public AttractionsServiceDb(AttractionsDbRepos repo)
    {
        _repo = repo;
    }

    public AttractionsServiceDb(AttractionsDbRepos repo, ILogger<AttractionsServiceDb> logger) : this(repo)
    {
        _logger = logger;
    }

    //Simple 1:1 calls in this case, but as Services expands, this will no longer need to be the case
    public Task<ResponsePageDto<IAttraction>> ReadAttractionsAsync() => _repo.ReadAttractionsAsync();
}