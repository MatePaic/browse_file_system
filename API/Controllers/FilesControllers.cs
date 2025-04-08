using API.DTO;
using API.Services;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController(IFileService _fileService, IFolderRepository _folderRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<GetFileDto>>> GetFiles()
        {
            var files = await _fileService.GetFilesAsync();

            return Ok(files);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetFileDto>> GetFileById(int id)
        {
            var file = await _fileService.GetFileByIdAsync(id);

            return file == null ? NotFound() : Ok(file);
        }

        [HttpPost]
        public async Task<ActionResult<ReturnFileDto>> CreateFile(FileCreateDto fileDto)
        {
            var allFolders = await _folderRepository.GetFoldersAsync();
            if (allFolders.Find(folder => folder.Id == fileDto.FolderId) == null) 
            {
                return BadRequest("Cannot create a file in a folder which does not exist!");
            }

            var createdFile = await _fileService.CreateFileAsync(fileDto);

            return CreatedAtAction(nameof(GetFileById), new { id = createdFile.Id }, createdFile);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var result = await _fileService.DeleteFileAsync(id);

            return result ? NoContent() : NotFound();
        }
    }
}
