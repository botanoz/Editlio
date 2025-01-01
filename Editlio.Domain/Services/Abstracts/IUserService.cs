using Editlio.Shared.DTOs.User;
using Editlio.Shared.Helpers;

namespace Editlio.Domain.Services.Abstracts
{
    public interface IUserService
    {
        Task<Result<UserDto>> GetUserByIdAsync(int userId);
        Task<Result<UserDto>> GetUserByEmailAsync(string email);
        Task<Result<List<UserDto>>> GetAllUsersAsync();
        Task<Result<UserDto>> CreateUserAsync(UserRegisterDto userDto);
        Task<Result<UserDto>> UpdateUserAsync(UserDto userDto);
        Task<Result> DeleteUserAsync(int userId);
    }
}
