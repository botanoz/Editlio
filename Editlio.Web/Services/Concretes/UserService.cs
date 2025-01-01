using Editlio.Shared.DTOs.User;
using Editlio.Web.Helpers;
using Editlio.Web.Models;
using Editlio.Web.Services.Abstracts;
using System.Net.Http.Json;

namespace Editlio.Web.Services.Concretes
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/user");
                if (response.IsSuccessStatusCode)
                {
                    var userDtos = await response.Content.ReadFromJsonAsync<List<UserDto>>();
                    return userDtos?.ToViewModelList() ?? new List<UserViewModel>();
                }
                Console.WriteLine("Failed to fetch all users.");
                return new List<UserViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all users: {ex.Message}");
                return new List<UserViewModel>();
            }
        }

        public async Task<UserViewModel?> GetUserByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/user/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var userDto = await response.Content.ReadFromJsonAsync<UserDto>();
                    return userDto?.ToViewModel();
                }
                Console.WriteLine($"Failed to fetch the user by ID: {id}.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user by ID {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateUserAsync(UserViewModel user)
        {
            try
            {
                var userDto = user.ToDto();
                var response = await _httpClient.PostAsJsonAsync("api/user", userDto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(UserViewModel user)
        {
            try
            {
                var userDto = user.ToDto();
                var response = await _httpClient.PutAsJsonAsync($"api/user/{user.Id}", userDto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user ID {user.Id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/user/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user ID {id}: {ex.Message}");
                return false;
            }
        }
    }
}
