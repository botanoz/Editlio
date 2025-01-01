using Editlio.Domain.Services.Abstracts;
using Editlio.Infrastructure.Repositories.Abstracts;
using Editlio.Shared.DTOs.RealTimeUpdate;
using Editlio.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Domain.Services.Concretes
{
    public class RealTimeUpdateService : IRealTimeUpdateService
    {
        private readonly IRealTimeUpdateRepository _realTimeUpdateRepository;

        public RealTimeUpdateService(IRealTimeUpdateRepository realTimeUpdateRepository)
        {
            _realTimeUpdateRepository = realTimeUpdateRepository;
        }

        public async Task<Result<RealTimeUpdateDto>> GetRealTimeUpdateByIdAsync(int updateId)
        {
            var updateResult = await _realTimeUpdateRepository.GetAsync(u => u.Id == updateId);
            return updateResult.Success
                ? Result<RealTimeUpdateDto>.SuccessResult(updateResult.Data!.ToDto())
                : Result<RealTimeUpdateDto>.FailureResult(new List<string> { "Real-time update not found" });
        }

        public async Task<Result<List<RealTimeUpdateDto>>> GetUpdatesByPageIdAsync(int pageId)
        {
            var updatesResult = await _realTimeUpdateRepository.GetListAsync(u => u.PageId == pageId);
            return updatesResult.Success
                ? Result<List<RealTimeUpdateDto>>.SuccessResult(updatesResult.Data!.ToDtoList())
                : Result<List<RealTimeUpdateDto>>.FailureResult(new List<string> { "No updates found for this page" });
        }

        public async Task<Result<RealTimeUpdateDto>> CreateRealTimeUpdateAsync(RealTimeUpdateCreateDto updateDto)
        {
            var update = updateDto.ToEntity();
            update.CreatedDate = System.DateTime.UtcNow;

            var createResult = await _realTimeUpdateRepository.AddAsync(update);
            return createResult.Success
                ? Result<RealTimeUpdateDto>.SuccessResult(createResult.Data!.ToDto())
                : Result<RealTimeUpdateDto>.FailureResult(new List<string> { "Failed to create real-time update" });
        }

        public async Task<Result<RealTimeUpdateDto>> UpdateRealTimeUpdateAsync(RealTimeUpdateUpdateDto updateDto)
        {
            var updateResult = await _realTimeUpdateRepository.GetAsync(u => u.Id == updateDto.Id);
            if (!updateResult.Success || updateResult.Data == null)
            {
                return Result<RealTimeUpdateDto>.FailureResult(new List<string> { "Real-time update not found" });
            }

            var update = updateDto.ToEntity(updateResult.Data);

            var updateResultFinal = await _realTimeUpdateRepository.UpdateAsync(update);
            return updateResultFinal.Success
                ? Result<RealTimeUpdateDto>.SuccessResult(updateResultFinal.Data!.ToDto())
                : Result<RealTimeUpdateDto>.FailureResult(new List<string> { "Failed to update real-time update" });
        }

        public async Task<Result> DeleteRealTimeUpdateAsync(int updateId)
        {
            var updateResult = await _realTimeUpdateRepository.GetAsync(u => u.Id == updateId);
            if (!updateResult.Success || updateResult.Data == null)
            {
                return Result.FailureResult(new List<string> { "Real-time update not found" });
            }

            var deleteResult = await _realTimeUpdateRepository.DeleteAsync(updateResult.Data);
            return deleteResult.Success
                ? Result.SuccessResult()
                : Result.FailureResult(new List<string> { "Failed to delete real-time update" });
        }
    }
}
