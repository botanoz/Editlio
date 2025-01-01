using Editlio.Shared.DTOs.RefreshToken;
using Editlio.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Domain.Services.Abstracts
{
    public interface IRefreshTokenService
    {
        Task<Result<RefreshTokenDto>> GetRefreshTokenByIdAsync(int tokenId);
        Task<Result<RefreshTokenDto>> GetRefreshTokenByValueAsync(string tokenValue);
        Task<Result<RefreshTokenDto>> CreateRefreshTokenAsync(RefreshTokenCreateDto tokenDto);
        Task<Result<RefreshTokenDto>> UpdateRefreshTokenAsync(RefreshTokenUpdateDto tokenDto);
        Task<Result> DeleteRefreshTokenAsync(int tokenId);
    }
}
