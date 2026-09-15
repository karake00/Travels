using Models;
using Models.DTO;

namespace Services;

public interface IUsersService
{
    public Task<ResponsePageDto<IUser>> ReadUsersAsync();
}