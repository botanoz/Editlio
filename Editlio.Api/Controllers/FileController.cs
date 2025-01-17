using Editlio.Domain.Services.Abstracts;
using Editlio.Domain.Services.Concretes;
using Editlio.Shared.DTOs.File;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Editlio.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IPageService _pageService;

        public FileController(IFileService fileService, IPageService pageService)
        {
            _fileService = fileService;
            _pageService = pageService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(FileCreateDto dto)
        {
            var result = await _fileService.CreateFileAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(FileUpdateDto dto)
        {
            var result = await _fileService.UpdateFileAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _fileService.GetFileByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }
        [HttpGet("Pageid/{id}")]
        public async Task<IActionResult> GetByPageId(int id)
        {
            var result = await _fileService.GetFilesByPageIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }
        [HttpGet("Slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var data = await _pageService.GetPageBySlugAsync(slug);
            var result = await _fileService.GetFilesByPageIdAsync(data.Data.Id);
            return result.Success ? Ok(result) : NotFound(result);
        }
        [HttpDelete("file/{id}")]
        public async Task<IActionResult> DeleteFileById(int id)
        {
            var result = await _fileService.DeleteFileAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
