using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

using DbContext;
using DbModels;
using Models;
using Models.DTO;

namespace DbRepos;

public class CountriesDbRepos
{
    private readonly ILogger<CountriesDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public CountriesDbRepos(ILogger<CountriesDbRepos> logger, MainDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<ResponsePageDto<ICountry>> ReadCountriesAsync()
    {
        IQueryable<CountryDbM> query = _dbContext.Countries.AsNoTracking();
        var ret = new ResponsePageDto<ICountry>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            DbItemsCount = await query.CountAsync(),
            PageItems = await query.ToListAsync<ICountry>()
        };
        return ret;
    }
}