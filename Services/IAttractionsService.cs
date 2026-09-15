using Models;
using Models.DTO;

namespace Services;

public interface IAttractionsService
{
    public Task<ResponsePageDto<IAttraction>> ReadAttractionsAsync();
}