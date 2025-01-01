using Editlio.Domain.Services.Abstracts;
using Editlio.Infrastructure.Repositories.Abstracts;
using Editlio.Shared.DTOs.RefreshToken;
using Editlio.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Domain.Services.Concretes
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Result<RefreshTokenDto>> GetRefreshTokenByIdAsync(int tokenId)
        {
            var tokenResult = await _refreshTokenRepository.GetAsync(t => t.Id == tokenId);
            return tokenResult.Success
                ? Result<RefreshTokenDto>.SuccessResult(tokenResult.Data!.ToDto())
                : Result<RefreshTokenDto>.FailureResult(new List<string> { "Refresh token not found" });
        }

        public async Task<Result<RefreshTokenDto>> GetRefreshTokenByValueAsync(string tokenValue)
        {
            var tokenResult = await _refreshTokenRepository.GetAsync(t => t.Token == tokenValue);
            return tokenResult.Success
                ? Result<RefreshTokenDto>.SuccessResult(tokenResult.Data!.ToDto())
                : Result<RefreshTokenDto>.FailureResult(new List<string> { "Refresh token not found" });
        }

        public async Task<Result<RefreshTokenDto>> CreateRefreshTokenAsync(RefreshTokenCreateDto tokenDto)
        {
            var token = tokenDto.ToEntity();
            token.CreatedDate = System.DateTime.UtcNow;

            var createResult = await _refreshTokenRepository.AddAsync(token);
            return createResult.Success
                ? Result<RefreshTokenDto>.SuccessResult(createResult.Data!.ToDto())
                : Result<RefreshTokenDto>.FailureResult(new List<string> { "Failed to create refresh token" });
        }

        public async Task<Result<RefreshTokenDto>> UpdateRefreshTokenAsync(RefreshTokenUpdateDto tokenDto)
        {
            var tokenResult = await _refreshTokenRepository.GetAsync(t => t.Id == tokenDto.Id);
            if (!tokenResult.Success || tokenResult.Data == null)
            {
                return Result<RefreshTokenDto>.FailureResult(new List<string> { "Refresh token not found" });
            }

            var token = tokenDto.ToEntity(tokenResult.Data);

            var updateResult = await _refreshTokenRepository.UpdateAsync(token);
            return updateResult.Success
                ? Result<RefreshTokenDto>.SuccessResult(updateResult.Data!.ToDto())
                : Result<RefreshTokenDto>.FailureResult(new List<string> { "Failed to update refresh token" });
        }

        public async Task<Result> DeleteRefreshTokenAsync(int tokenId)
        {
            var tokenResult = await _refreshTokenRepository.GetAsync(t => t.Id == tokenId);
            if (!tokenResult.Success || tokenResult.Data == null)
            {
                return Result.FailureResult(new List<string> { "Refresh token not found" });
            }

            var deleteResult = await _refreshTokenRepository.DeleteAsync(tokenResult.Data);
            return deleteResult.Success
                ? Result.SuccessResult()
                : Result.FailureResult(new List<string> { "Failed to delete refresh token" });
        }
    }
}
