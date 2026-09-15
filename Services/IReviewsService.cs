using Models;
using Models.DTO;

namespace Services;

public interface IReviewsService
{
    public Task<ResponsePageDto<IReview>> ReadReviewsAsync();
}