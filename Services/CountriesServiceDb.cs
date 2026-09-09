using Microsoft.Extensions.Logging;

using Models;
using Models.DTO;
using DbRepos;

namespace Services;

public class CountriesServiceDb : ICountriesService
{
    private readonly CountriesDbRepos _repo = null;
    private readonly ILogger<CountriesServiceDb> _logger = null;

    public CountriesServiceDb(CountriesDbRepos repo)
    {
        _repo = repo;
    }

    public CountriesServiceDb(CountriesDbRepos repo, ILogger<CountriesServiceDb> logger) : this(repo)
    {
        _logger = logger;
    }

    //Simple 1:1 calls in this case, but as Services expands, this will no longer need to be the case
    public Task<ResponsePageDto<ICountry>> ReadCountriesAsync() => _repo.ReadCountriesAsync();
}