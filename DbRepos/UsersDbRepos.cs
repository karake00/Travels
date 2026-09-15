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

    public async Task<ResponsePageDto<IUser>> ReadUsersAsync()
    {
        IQueryable<UserDbM> query = _dbContext.Users.AsNoTracking();
        var ret = new ResponsePageDto<IUser>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            DbItemsCount = await query.CountAsync(),
            PageItems = await query.ToListAsync<IUser>(),
        };
        return ret;
    }
}