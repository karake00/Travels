using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using Models.DTO;
using DbModels;
using DbContext;

namespace DbRepos;

public class UsersDbRepos
{
    private ILogger<UsersDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public UsersDbRepos(ILogger<UsersDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }

    public async Task<ResponsePageDto<IUser>> ReadUsersAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        filter ??="";
        IQueryable<UserDbM> query = _dbContext.Users.AsNoTracking();

        if (!flat)
        {
            query = query
                .Include(u => u.ReviewsDbM);
        }
        
        var filteredQuery = query.Where(i => i.Seeded == seeded && 
            i.User_Name.ToLower().Contains(filter.ToLower()));

        var ret = new ResponsePageDto<IUser>()
        {   
#if DEBUG
        ConnectionString = _dbContext.dbConnection,
#endif

        DbItemsCount = await filteredQuery.CountAsync(),
        PageItems = await filteredQuery
        .Skip(pageNumber * pageSize)
        .Take(pageSize)
        .ToListAsync<IUser>(),
        
        PageNr = pageNumber,
        PageSize = pageSize
        };
        
        return ret;
    }
}