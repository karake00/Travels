using Models;
using Models.DTO;

namespace Services;

public interface ICitiesService
{
    public Task<ResponsePageDto<ICity>> ReadCitiesAsync();
}