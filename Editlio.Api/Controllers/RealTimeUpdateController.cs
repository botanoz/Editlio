using Editlio.Domain.Services.Abstracts;
using Editlio.Shared.DTOs.RealTimeUpdate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Editlio.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RealTimeUpdateController : ControllerBase
    {
        private readonly IRealTimeUpdateService _realTimeUpdateService;

        public RealTimeUpdateController(IRealTimeUpdateService realTimeUpdateService)
        {
            _realTimeUpdateService = realTimeUpdateService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(RealTimeUpdateCreateDto dto)
        {
            var result = await _realTimeUpdateService.CreateRealTimeUpdateAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(RealTimeUpdateUpdateDto dto)
        {
            var result = await _realTimeUpdateService.UpdateRealTimeUpdateAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _realTimeUpdateService.GetRealTimeUpdateByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
