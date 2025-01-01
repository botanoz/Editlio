using Editlio.Web.Models;

namespace Editlio.Web.Services.Abstracts
{
    public interface IUserService
    {
        Task<List<UserViewModel>> GetAllUsersAsync();
        Task<UserViewModel?> GetUserByIdAsync(int id);
        Task<bool> CreateUserAsync(UserViewModel user);
        Task<bool> UpdateUserAsync(UserViewModel user);
        Task<bool> DeleteUserAsync(int id);
    }
}
