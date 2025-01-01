using Editlio.Domain.Services.Abstracts;
using Editlio.Infrastructure.Repositories.Abstracts;
using Editlio.Shared.DTOs.User;
using Editlio.Shared.Entities;
using Editlio.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Domain.Services.Concretes
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserDto>> GetUserByIdAsync(int userId)
        {
            var userResult = await _userRepository.GetAsync(u => u.Id == userId);
            return userResult.Success
                ? Result<UserDto>.SuccessResult(userResult.Data!.ToDto())
                : Result<UserDto>.FailureResult(new List<string> { "User not found" });
        }

        public async Task<Result<UserDto>> GetUserByEmailAsync(string email)
        {
            var userResult = await _userRepository.GetAsync(u => u.Email == email);
            return userResult.Success
                ? Result<UserDto>.SuccessResult(userResult.Data!.ToDto())
                : Result<UserDto>.FailureResult(new List<string> { "User not found" });
        }

        public async Task<Result<List<UserDto>>> GetAllUsersAsync()
        {
            var userListResult = await _userRepository.GetListAsync();
            return userListResult.Success
                ? Result<List<UserDto>>.SuccessResult(userListResult.Data!.ToDtoList())
                : Result<List<UserDto>>.FailureResult(new List<string> { "No users found" });
        }

        public async Task<Result<UserDto>> CreateUserAsync(UserRegisterDto userDto)
        {
            var user = userDto.ToEntity();
            user.CreatedDate = System.DateTime.UtcNow;

            var createResult = await _userRepository.AddAsync(user);
            return createResult.Success
                ? Result<UserDto>.SuccessResult(createResult.Data!.ToDto())
                : Result<UserDto>.FailureResult(new List<string> { "Failed to create user" });
        }

        public async Task<Result<UserDto>> UpdateUserAsync(UserDto userDto)
        {
            var userResult = await _userRepository.GetAsync(u => u.Id == userDto.Id);
            if (!userResult.Success || userResult.Data == null)
            {
                return Result<UserDto>.FailureResult(new List<string> { "User not found" });
            }

            var user = userDto.ToEntity(userResult.Data);
            user.UpdatedDate = System.DateTime.UtcNow;

            var updateResult = await _userRepository.UpdateAsync(user);
            return updateResult.Success
                ? Result<UserDto>.SuccessResult(updateResult.Data!.ToDto())
                : Result<UserDto>.FailureResult(new List<string> { "Failed to update user" });
        }

        public async Task<Result> DeleteUserAsync(int userId)
        {
            var userResult = await _userRepository.GetAsync(u => u.Id == userId);
            if (!userResult.Success || userResult.Data == null)
            {
                return Result.FailureResult(new List<string> { "User not found" });
            }

            var deleteResult = await _userRepository.DeleteAsync(userResult.Data);
            return deleteResult.Success
                ? Result.SuccessResult()
                : Result.FailureResult(new List<string> { "Failed to delete user" });
        }
    }
}
