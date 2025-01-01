using Editlio.Domain.Services.Abstracts;
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

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
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
    }
}
