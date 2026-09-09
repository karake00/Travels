using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

using DbContext;
using DbModels;
using Models;
using Models.DTO;

namespace DbRepos;

public class CitiesDbRepos
{
    private readonly ILogger<CitiesDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public CitiesDbRepos(ILogger<CitiesDbRepos> logger, MainDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<ResponsePageDto<ICity>> ReadCitiesAsync()
    {
        IQueryable<CityDbM> query = _dbContext.Cities.AsNoTracking();
        var ret = new ResponsePageDto<ICity>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            DbItemsCount = await query.CountAsync(),
            PageItems = await query.ToListAsync<ICity>()
        };
        return ret;
    }
}