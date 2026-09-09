using Microsoft.Extensions.Logging;

using Models;
using Models.DTO;
using DbRepos;

namespace Services;

public class CitiesServiceDb : ICitiesService
{
    private readonly CitiesDbRepos _repo = null;
    private readonly ILogger<CitiesServiceDb> _logger = null;

    public CitiesServiceDb(CitiesDbRepos repo)
    {
        _repo = repo;
    }

    public CitiesServiceDb(CitiesDbRepos repo, ILogger<CitiesServiceDb> logger) : this(repo)
    {
        _logger = logger;
    }

    //Simple 1:1 calls in this case, but as Services expands, this will no longer need to be the case
    public Task<ResponsePageDto<ICity>> ReadCitiesAsync() => _repo.ReadCitiesAsync();
}