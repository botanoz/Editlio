using Editlio.Shared.DTOs.RealTimeUpdate;
using Editlio.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Domain.Services.Abstracts
{
    public interface IRealTimeUpdateService
    {
        Task<Result<RealTimeUpdateDto>> GetRealTimeUpdateByIdAsync(int updateId);
        Task<Result<List<RealTimeUpdateDto>>> GetUpdatesByPageIdAsync(int pageId);
        Task<Result<RealTimeUpdateDto>> CreateRealTimeUpdateAsync(RealTimeUpdateCreateDto updateDto);
        Task<Result<RealTimeUpdateDto>> UpdateRealTimeUpdateAsync(RealTimeUpdateUpdateDto updateDto);
        Task<Result> DeleteRealTimeUpdateAsync(int updateId);
    }
}
