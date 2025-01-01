using Editlio.Domain.Services.Abstracts;
using Editlio.Shared.DTOs.RefreshToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Editlio.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefreshTokenController : ControllerBase
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public RefreshTokenController(IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(RefreshTokenCreateDto dto)
        {
            var result = await _refreshTokenService.CreateRefreshTokenAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(RefreshTokenUpdateDto dto)
        {
            var result = await _refreshTokenService.UpdateRefreshTokenAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _refreshTokenService.GetRefreshTokenByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
