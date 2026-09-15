using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using Models.DTO;
using DbModels;
using DbContext;

namespace DbRepos;

public class ReviewsDbRepos
{
    private ILogger<ReviewsDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public ReviewsDbRepos(ILogger<ReviewsDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }

    public async Task<ResponsePageDto<IReview>> ReadReviewsAsync()
    {
        IQueryable<ReviewDbM> query = _dbContext.Reviews.AsNoTracking();
        var ret = new ResponsePageDto<IReview>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            DbItemsCount = await query.CountAsync(),
            PageItems = await query.ToListAsync<IReview>(),
        };
        return ret;
    }
}