using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using Models.DTO;
using DbModels;
using DbContext;

namespace DbRepos;

public class AttractionsDbRepos
{
    private ILogger<AttractionsDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public AttractionsDbRepos(ILogger<AttractionsDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }

    public async Task<ResponsePageDto<IAttraction>> ReadAttractionsAsync()
    {
        IQueryable<AttractionDbM> query = _dbContext.Attractions.AsNoTracking();
        var ret = new ResponsePageDto<IAttraction>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            DbItemsCount = await query.CountAsync(),
            PageItems = await query.ToListAsync<IAttraction>(),
        };
        return ret;
    }
}