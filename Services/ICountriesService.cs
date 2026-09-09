using Models;
using Models.DTO;

namespace Services;

public interface ICountriesService
{
    public Task<ResponsePageDto<ICountry>> ReadCountriesAsync();
}