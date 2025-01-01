using Editlio.Domain.Services.Abstracts;
using Editlio.Shared.DTOs.Page;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Editlio.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PageController : ControllerBase
    {
        private readonly IPageService _pageService;

        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(PageCreateDto dto)
        {
            var result = await _pageService.CreatePageAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(PageUpdateDto dto)
        {
            var result = await _pageService.UpdatePageAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _pageService.GetPageByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserPages(int userId)
        {
            var result = await _pageService.GetUserPagesAsync(userId);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
