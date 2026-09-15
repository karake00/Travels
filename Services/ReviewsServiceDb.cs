using Microsoft.Extensions.Logging;

using Models;
using Models.DTO;
using DbRepos;

namespace Services;

public class ReviewsServiceDb : IReviewsService
{
    private readonly ReviewsDbRepos _repo = null;
    private readonly ILogger<ReviewsServiceDb> _logger = null;

    public ReviewsServiceDb(ReviewsDbRepos repo)
    {
        _repo = repo;
    }

    public ReviewsServiceDb(ReviewsDbRepos repo, ILogger<ReviewsServiceDb> logger) : this(repo)
    {
        _logger = logger;
    }

    //Simple 1:1 calls in this case, but as Services expands, this will no longer need to be the case
    public Task<ResponsePageDto<IReview>> ReadReviewsAsync() => _repo.ReadReviewsAsync();
}